using System.Globalization;
using System.Xml.Linq;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;

namespace ResxManagerExtended.Desktop.Data;

public class ResxFile : IResourceFile
{
    public IReadOnlyDictionary<CultureInfo, string>? Paths { get; init; }

    public string? RelativePath { get; set; }

    public required string Path { get; init; }

    public required string Name { get; init; }

    public IEnumerable<CultureInfo>? Cultures { get; init; }

    public Task SetValue(string key, IDictionary<CultureInfo, string?> cultures)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ResourceView>> GetValues(CancellationToken token)
    {
        var resources = new Dictionary<string, ResourceView>();

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
                        Path = this.GetFullPath(),
                        Key = key,
                        Columns = new Dictionary<CultureInfo, string?> { { culture, key } }
                    });

                if (string.IsNullOrEmpty(culture.Name))
                    resources[key].Comment = comment;
            }
        }

        return Task.FromResult<IEnumerable<ResourceView>>(resources.Values);
    }
}