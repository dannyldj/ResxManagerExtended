using System.Globalization;

namespace ResxManagerExtended.Shared.Data;

public class ResourceView
{
    public ResourceView(string key, CultureInfo culture, string? value)
    {
        Key = key;
        Columns[culture] = value;
    }

    public string Key { get; set; }

    public Dictionary<CultureInfo, string?> Columns { get; } = [];
}