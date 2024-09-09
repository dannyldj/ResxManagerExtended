using KristofferStrube.Blazor.FileSystem;
using ResxManagerExtended.Web.Data;

namespace ResxManagerExtended.Web.Extensions;

public static class FileSystemDirectoryHandleExtension
{
    private static readonly IEnumerable<string> ResourceExtensions = [".resx"];

    public static async Task<ResourceTreeViewItem?> GetResourceFiles(
        this FileSystemDirectoryHandleInProcess handle)
    {
        var topNode = new ResourceTreeViewItem(await handle.GetNameAsync(), handle);
        var nodes = new List<ResourceTreeViewItem>();
        var procCount = Environment.ProcessorCount;
        var values = await handle.ValuesAsync();

        if (values.Length < procCount)
            foreach (var entry in values)
            {
                switch (entry.Kind)
                {
                    case FileSystemHandleKind.File when ResourceExtensions.Contains(Path.GetExtension(entry.Name)):
                        nodes.Add(new ResourceTreeViewItem(entry.Name));
                        break;
                    case FileSystemHandleKind.File:
                        // Non-resource files
                        break;
                    case FileSystemHandleKind.Directory:
                        var directoryHandle = await handle.GetDirectoryHandleAsync(entry.Name);
                        var childNode = await directoryHandle.GetResourceFiles();
                        if (childNode is not null) nodes.Add(childNode);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(handle));
                }
            }
        else
            await Parallel.ForEachAsync(values, async (entry, token) =>
            {
                if (token.IsCancellationRequested) return;

                switch (entry.Kind)
                {
                    case FileSystemHandleKind.File when ResourceExtensions.Contains(Path.GetExtension(entry.Name)):
                        nodes.Add(new ResourceTreeViewItem(entry.Name));
                        break;
                    case FileSystemHandleKind.File:
                        // Non-resource files
                        break;
                    case FileSystemHandleKind.Directory:
                        var directoryHandle = await handle.GetDirectoryHandleAsync(entry.Name);
                        var childNode = await directoryHandle.GetResourceFiles();
                        if (childNode is not null) nodes.Add(childNode);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(handle));
                }
            });

        if (nodes.Count == 0) return null;

        topNode.Items = nodes;
        return topNode;
    }
}