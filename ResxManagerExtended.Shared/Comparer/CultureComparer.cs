using System.Globalization;

namespace ResxManagerExtended.Shared.Comparer;

public class CultureComparer : IComparer<CultureInfo>
{
    public int Compare(CultureInfo? x, CultureInfo? y)
    {
        return string.Compare(x?.Name, y?.Name, StringComparison.Ordinal);
    }
}