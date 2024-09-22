using System.Globalization;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Extensions;

public static class ResxExtension
{
    private const string FileExtension = ".resx";

    public static string GetResxFileName(this CultureInfo culture, string name)
    {
        return string.IsNullOrEmpty(culture.Name) ? $"{name}{FileExtension}" : $"{name}.{culture.Name}{FileExtension}";
    }

    public static string GetFullPath(this IResourceFile resource)
    {
        return $"{resource.Path}{Path.DirectorySeparatorChar}{resource.Name}";
    }

    public static bool IsUnderDirectory(this string firstPath, string secondPath)
    {
        if (firstPath.EndsWith(Path.DirectorySeparatorChar.ToString()) is false)
            firstPath += Path.DirectorySeparatorChar;

        if (secondPath.EndsWith(Path.DirectorySeparatorChar.ToString()) is false)
            secondPath += Path.DirectorySeparatorChar;

        return firstPath.StartsWith(secondPath, StringComparison.OrdinalIgnoreCase);
    }
}