using System.Globalization;
using System.Xml.Linq;
using KristofferStrube.Blazor.FileSystem;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;

namespace ResxManagerExtended.Web.Data;

public class ResxFile : IResourceFile
{
    public IReadOnlyDictionary<CultureInfo, FileSystemFileHandleInProcess>? Handles { get; init; }
    public required string Path { get; init; }

    public required string Name { get; init; }

    public IEnumerable<CultureInfo>? Cultures { get; init; }

    public async Task<IEnumerable<ResourceView>> GetValues(CancellationToken token)
    {
        var resources = new Dictionary<string, ResourceView>();

        foreach (var culture in Cultures ?? [])
        {
            if (Handles?.TryGetValue(culture, out var handle) is not true) continue;

            await using var file = await handle.GetFileAsync();
            await using var stream = await file.StreamAsync();

            var document = await XDocument.LoadAsync(stream, LoadOptions.None, token);
            foreach (var (key, value) in document.GetResources(culture))
            {
                if (resources.TryGetValue(key, out var view))
                    view.Columns[culture] = value;
                else
                    resources.Add(key, new ResourceView(key, culture, value));
            }
        }

        return resources.Values;
    }
}