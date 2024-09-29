using System.Globalization;
using System.Text;
using System.Xml.Linq;
using KristofferStrube.Blazor.FileSystem;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using static System.IO.Path;

namespace ResxManagerExtended.Web.Data;

public class ResxFile : IResourceFile
{
    public IReadOnlyDictionary<CultureInfo, FileSystemFileHandleInProcess>? Handles { get; init; }

    public required string Path { get; init; }

    public required string Name { get; init; }

    public IEnumerable<CultureInfo>? Cultures { get; init; }

    public string GetResourcePath()
    {
        return $"{Path}{DirectorySeparatorChar}{Name}";
    }

    public async Task SetValue(string key, CultureInfo culture, string value, CancellationToken token)
    {
        if (Handles?.TryGetValue(culture, out var handle) is not true) return;

        await using var file = await handle.GetFileAsync();
        var xml = await file.TextAsync();

        var document = XDocument.Parse(xml);
        document.GetNode(key)?.SetValue(value);

        await using var writable = await handle.CreateWritableAsync();
        await using var writer = new StreamWriter(writable,
            new UTF8Encoding(IResourceFile.DetectUtf8Bom(await file.ArrayBufferAsync())));
        await document.SaveAsync(writer, SaveOptions.None, token);
    }

    public async Task SetValue(string key, IDictionary<CultureInfo, string?> cultures, CancellationToken token)
    {
        foreach (var (culture, value) in cultures)
        {
            await SetValue(key, culture, value ?? string.Empty, token);
        }
    }

    public async Task<IEnumerable<ResourceView>> GetValues(CancellationToken token)
    {
        var resources = new Dictionary<string, ResourceView>();

        foreach (var culture in Cultures ?? [])
        {
            if (Handles?.TryGetValue(culture, out var handle) is not true) continue;

            await using var file = await handle.GetFileAsync();
            var xml = await file.TextAsync();

            var document = XDocument.Parse(xml);
            foreach (var (key, comment, value) in document.GetResources())
            {
                if (resources.TryGetValue(key, out var view))
                    view.Columns[culture] = value;
                else
                    resources.Add(key, new ResourceView
                    {
                        Path = GetResourcePath(),
                        Key = key,
                        Columns = new Dictionary<CultureInfo, string?> { { culture, value } }
                    });

                if (string.IsNullOrEmpty(culture.Name))
                    resources[key].Comment = comment;
            }
        }

        return resources.Values;
    }
}