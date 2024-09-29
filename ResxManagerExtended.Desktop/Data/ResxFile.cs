using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using static System.IO.Path;

namespace ResxManagerExtended.Desktop.Data;

public class ResxFile : IResourceFile
{
    public IReadOnlyDictionary<CultureInfo, string>? Paths { get; init; }

    public string? RelativePath { get; set; }

    public required string Path { get; init; }

    public required string Name { get; init; }

    public IEnumerable<CultureInfo>? Cultures { get; init; }

    public string GetResourcePath()
    {
        return $"{RelativePath}{DirectorySeparatorChar}{Name}";
    }

    public async Task SetValue(string key, CultureInfo culture, string value, CancellationToken token)
    {
        if (Paths?.TryGetValue(culture, out var path) is not true) return;

        var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
        document.GetNode(key)?.SetValue(value);

        await using var writer = new StreamWriter(path, false, new UTF8Encoding(IResourceFile.DetectUtf8Bom(path)));
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

        await Task.Yield();
        foreach (var culture in Cultures ?? [])
        {
            if (Paths?.TryGetValue(culture, out var path) is not true) continue;

            var document = XDocument.Load(path);
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