using System.Globalization;
using System.Xml.Linq;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using static System.IO.Path;

namespace ResxManagerExtended.Desktop.Data;

public class ResxFile : IResourceFile
{
    public required string Path { get; init; }

    public required string Name { get; init; }

    public IEnumerable<CultureInfo>? Cultures { get; init; }

    public Task<IEnumerable<ResourceView>> GetValues(CancellationToken token)
    {
        var resources = new Dictionary<string, ResourceView>();

        foreach (var culture in Cultures ?? [])
        {
            var document = XDocument.Load(Path + DirectorySeparatorChar + culture.GetResxFileName(Name));
            foreach (var (key, comment, value) in document.GetResources())
            {
                if (resources.TryGetValue(key, out var view))
                    view.Columns[culture] = value;
                else
                    resources.Add(key, new ResourceView(Path + DirectorySeparatorChar + Name, key, culture, value));

                if (string.IsNullOrEmpty(culture.Name))
                    resources[key].Comment = comment;
            }
        }

        return Task.FromResult<IEnumerable<ResourceView>>(resources.Values);
    }
}