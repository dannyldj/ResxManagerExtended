using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Extensions;

public static class ResxExtension
{
    public static string GetFullPath(this IResourceFile resource)
    {
        return $"{resource.Path}{Path.DirectorySeparatorChar}{resource.Name}";
    }

    public static string GetRelativePath(this IResourceFile resource, string rootPath)
    {
        return resource.Path.Remove(0, (Path.GetDirectoryName(rootPath) ?? "").Length + 1);
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