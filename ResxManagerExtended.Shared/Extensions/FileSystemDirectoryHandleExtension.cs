using KristofferStrube.Blazor.FileSystem;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Extensions;

public static class FileSystemDirectoryHandleExtension
{
    private static readonly IEnumerable<string> ResourceExtensions = [".resx"];

    public static async Task<IEnumerable<ITreeViewItem>> GetResourceFiles(
        this FileSystemDirectoryHandleInProcess handle)
    {
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
                        var children = await directoryHandle.GetResourceFiles() as List<ResourceTreeViewItem>;

                        if (children?.Count > 0)
                            nodes.Add(new ResourceTreeViewItem(entry.Name, directoryHandle, children));

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
                        var children = await directoryHandle.GetResourceFiles() as List<ResourceTreeViewItem>;

                        if (children?.Count > 0)
                            nodes.Add(new ResourceTreeViewItem(entry.Name, directoryHandle, children));

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(handle));
                }
            });

        return nodes;
    }
}