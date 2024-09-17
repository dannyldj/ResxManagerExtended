using System.Collections;
using System.Globalization;
using System.Resources.NetStandard;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using static System.IO.Path;

namespace ResxManagerExtended.Desktop.Data;

public class ResxFile : IResourceFile
{
    public required string Path { get; init; }

    public required string Name { get; init; }

    public IEnumerable<CultureInfo>? Cultures { get; init; }

    public Task<IEnumerable<ResourceView>> GetValues()
    {
        var resources = new Dictionary<string, ResourceView>();

        foreach (var culture in Cultures ?? [])
        {
            using var resxReader =
                new ResXResourceReader(Path + DirectorySeparatorChar + culture.GetResxFileName(Name));

            if (string.IsNullOrEmpty(culture.Name))
                foreach (DictionaryEntry entry in resxReader)
                {
                    var key = entry.Key.ToString();
                    if (key is null) continue;

                    if (resources.TryGetValue(key, out var view))
                        view.DefaultValue = entry.Value?.ToString();
                    else
                        resources.Add(key,
                            new ResourceView { Key = key, DefaultValue = entry.Value?.ToString() });
                }
            else
                foreach (DictionaryEntry entry in resxReader)
                {
                    var key = entry.Key.ToString();
                    if (key is null) continue;

                    if (resources.TryGetValue(key, out var view))
                        view.AdditionalColumns[culture] = entry.Value?.ToString();
                    else
                        resources.Add(key,
                            new ResourceView(culture, entry.Value?.ToString()) { Key = key });
                }
        }

        return Task.FromResult<IEnumerable<ResourceView>>(resources.Values);
    }
}