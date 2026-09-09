using System.Globalization;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Extensions;

public static class ResxExtension
{
    extension(IResourceFile resource)
    {
        public string GetFullPath()
        {
            return $"{resource.Path}{Path.DirectorySeparatorChar}{resource.Name}";
        }

        public ResourceView CreateResourceView(string key)
        {
            var columns = new Dictionary<CultureInfo, string?>();
            foreach (var culture in resource.Cultures ?? [])
            {
                columns.TryAdd(culture, null);
            }

            return new ResourceView { Path = resource.GetResourcePath(), Key = key, Columns = columns };
        }

        public string GetRelativePath(string rootPath)
        {
            return resource.Path.Remove(0, (Path.GetDirectoryName(rootPath) ?? "").Length + 1);
        }
    }

    public static bool IsUnderDirectory(this string firstPath, string secondPath)
    {
        if (!firstPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            firstPath += Path.DirectorySeparatorChar;
        }

        if (!secondPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            secondPath += Path.DirectorySeparatorChar;
        }

        return firstPath.StartsWith(secondPath, StringComparison.OrdinalIgnoreCase);
    }
}