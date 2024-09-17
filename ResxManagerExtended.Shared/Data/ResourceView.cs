using System.Globalization;

namespace ResxManagerExtended.Shared.Data;

public class ResourceView
{
    public ResourceView()
    {
    }

    public ResourceView(CultureInfo culture, string? value)
    {
        AdditionalColumns[culture] = value;
    }

    public required string Key { get; set; }

    public string? DefaultValue { get; set; }

    public Dictionary<CultureInfo, string?> AdditionalColumns { get; } = new();
}