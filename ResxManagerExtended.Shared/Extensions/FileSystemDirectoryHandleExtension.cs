using KristofferStrube.Blazor.FileSystem;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Extensions;

public static class FileSystemDirectoryHandleExtension
{
    private static readonly IEnumerable<string> ResourceExtensions = [".resx"];

    public static async Task<IEnumerable<ResourceTreeViewItem>> GetResourceFiles(
        this FileSystemDirectoryHandleInProcess handle)
    {
        return await GetResourcesRecursive(handle, handle.Name);
    }

    private static async Task<IEnumerable<ResourceTreeViewItem>> GetResourcesRecursive(
        FileSystemDirectoryHandleInProcess handle, string path)
    {
        List<ResourceTreeViewItem> tree = [];

        await Parallel.ForEachAsync(await handle.ValuesAsync(), async (entry, token) =>
        {
            if (token.IsCancellationRequested) return;

            switch (entry.Kind)
            {
                case FileSystemHandleKind.File when ResourceExtensions.Contains(Path.GetExtension(entry.Name)):
                    tree.Add(new ResourceTreeViewItem(entry.Name));
                    break;
                case FileSystemHandleKind.File:
                    // Non-resource files
                    break;
                case FileSystemHandleKind.Directory:
                    var directoryHandle = await handle.GetDirectoryHandleAsync(entry.Name);
                    var children =
                        await GetResourcesRecursive(directoryHandle, $"{path}/{entry.Name}") as
                            List<ResourceTreeViewItem>;

                    if (children?.Count > 0) tree.Add(new ResourceTreeViewItem(entry.Name, directoryHandle, children));

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(handle));
            }
        });

        return tree;
    }
}