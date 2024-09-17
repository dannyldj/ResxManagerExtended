using System.Globalization;

namespace ResxManagerExtended.Shared.Data;

public interface IResourceFile
{
    string Path { get; init; }

    string Name { get; init; }

    IEnumerable<CultureInfo>? Cultures { get; init; }

    Task<IEnumerable<ResourceView>> GetValues();
}