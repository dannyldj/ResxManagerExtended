using System.Globalization;

namespace ResxManagerExtended.Shared.Data;

public interface IResourceFile
{
    string Path { get; }

    string Name { get; }

    IEnumerable<CultureInfo>? Cultures { get; }

    Task SetValue(string key, IDictionary<CultureInfo, string?> cultures);

    Task<IEnumerable<ResourceView>> GetValues(CancellationToken token = default);
}