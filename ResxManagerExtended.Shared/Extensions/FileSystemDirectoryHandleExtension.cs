using KristofferStrube.Blazor.FileSystem;

namespace ResxManagerExtended.Shared.Extensions;

public static class FileSystemDirectoryHandleExtension
{
    private static readonly IEnumerable<string> ResourceExtensions = [".resx"];

    public static async Task<IEnumerable<FileSystemFileHandleInProcess>> GetResourceFiles(
        this FileSystemDirectoryHandleInProcess handle)
    {
        return await GetResourcesRecursive(handle, handle.Name);
    }

    private static async Task<IEnumerable<FileSystemFileHandleInProcess>> GetResourcesRecursive(
        FileSystemDirectoryHandleInProcess handle, string path)
    {
        List<FileSystemFileHandleInProcess> resources = [];

        foreach (var entry in await handle.ValuesAsync())
        {
            var nestedPath = $"{path}/{entry.Name}";
            switch (entry.Kind)
            {
                case FileSystemHandleKind.File when ResourceExtensions.Contains(Path.GetExtension(entry.Name)):
                    resources.Add(await handle.GetFileHandleAsync(entry.Name));
                    break;
                case FileSystemHandleKind.Directory:
                    resources.AddRange(await GetResourcesRecursive(await handle.GetDirectoryHandleAsync(entry.Name),
                        nestedPath));
                    break;
            }
        }

        return resources;
    }
}