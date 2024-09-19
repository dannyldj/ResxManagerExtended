using System.Globalization;

namespace ResxManagerExtended.Shared.Data;

public class ResourceView
{
    public ResourceView(string path, string key, CultureInfo culture, string? value)
    {
        Path = path;
        Key = key;
        Columns[culture] = value;
    }

    public string Path { get; set; }

    public string Key { get; set; }

    public string? Comment { get; set; }

    public Dictionary<CultureInfo, string?> Columns { get; } = [];
}