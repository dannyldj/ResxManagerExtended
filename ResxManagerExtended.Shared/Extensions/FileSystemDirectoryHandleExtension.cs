using KristofferStrube.Blazor.FileSystem;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Extensions;

public static class FileSystemDirectoryHandleExtension
{
    private static readonly IEnumerable<string> ResourceExtensions = [".resx"];

    public static async Task<IEnumerable<Resource>> GetResourceFiles(
        this FileSystemDirectoryHandleInProcess handle)
    {
        return await GetResourcesRecursive(handle, handle.Name);
    }

    private static async Task<IEnumerable<Resource>> GetResourcesRecursive(
        FileSystemDirectoryHandleInProcess handle, string path)
    {
        List<Resource> directories = [];
        List<FileSystemFileHandleInProcess> resources = [];

        foreach (var entry in await handle.ValuesAsync())
            switch (entry.Kind)
            {
                case FileSystemHandleKind.File when ResourceExtensions.Contains(Path.GetExtension(entry.Name)):
                    resources.Add(await handle.GetFileHandleAsync(entry.Name));
                    break;
                case FileSystemHandleKind.Directory:
                    directories.AddRange(await GetResourcesRecursive(await handle.GetDirectoryHandleAsync(entry.Name),
                        $"{path}/{entry.Name}"));
                    break;
            }

        return resources.Count != 0
            ? [.. directories, new Resource(path, handle, resources)]
            : directories;
    }
}