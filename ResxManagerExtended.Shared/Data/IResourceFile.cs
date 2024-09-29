using System.Globalization;

namespace ResxManagerExtended.Shared.Data;

public interface IResourceFile
{
    string Path { get; }

    string Name { get; }

    IEnumerable<CultureInfo>? Cultures { get; }

    string GetResourcePath();

    Task<IEnumerable<ResourceView>> GetValues(CancellationToken token = default);
}