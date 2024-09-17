using System.Globalization;

namespace ResxManagerExtended.Shared.Extensions;

public static class ResxExtension
{
    public const string FileExtension = ".resx";

    public static string GetResxFileName(this CultureInfo culture, string name)
    {
        return string.IsNullOrEmpty(culture.Name) ? $"{name}{FileExtension}" : $"{name}.{culture.Name}{FileExtension}";
    }

    public static bool IsUnderDirectory(this string firstPath, string secondPath)
    {
        if (File.Exists(secondPath))
            return firstPath.Equals(secondPath);

        if (firstPath.EndsWith(Path.DirectorySeparatorChar.ToString()) is false)
            firstPath += Path.DirectorySeparatorChar;

        if (secondPath.EndsWith(Path.DirectorySeparatorChar.ToString()) is false)
            secondPath += Path.DirectorySeparatorChar;

        return firstPath.StartsWith(secondPath, StringComparison.OrdinalIgnoreCase);
    }
}