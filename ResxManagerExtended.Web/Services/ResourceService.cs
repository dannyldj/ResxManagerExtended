using System.Collections.Concurrent;
using System.Globalization;
using System.Text.RegularExpressions;
using Fluxor;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Services;
using ResxManagerExtended.Shared.Store.ResxManager;
using ResxManagerExtended.Shared.Store.Settings.UseCase;
using ResxManagerExtended.Web.Data;

namespace ResxManagerExtended.Web.Services;

internal class ResourceService(
    IFileSystemAccessServiceInProcess fileSystemAccessService,
    IDispatcher dispatcher,
    IState<SettingState> settingState) : IResourceService
{
    private record Resource(CultureInfo Culture, FileSystemFileHandleInProcess Handle);

    public async Task<ITreeViewItem?> SetTopNode()
    {
        try
        {
            await using var handle = await fileSystemAccessService.ShowDirectoryPickerAsync();

            dispatcher.Dispatch(new SetResourcesAction(null));

            return new TreeViewItem
            {
                Text = handle.Name,
                Items = await GetTreeItems(handle.Name, handle),
                IconCollapsed = new Icons.Regular.Size20.Folder(),
                IconExpanded = new Icons.Regular.Size20.FolderOpen(),
                Expanded = true
            };
        }
        catch (JSException)
        {
            // Closing the DirectoryPicker throws an exception.
            return null;
        }
    }

    private async Task<List<ITreeViewItem>> GetTreeItems(string directoryPath,
        FileSystemDirectoryHandleInProcess handle)
    {
        var items = new List<ITreeViewItem>();
        var resources = new ConcurrentDictionary<string, IEnumerable<Resource>>();
        var regex = new Regex(settingState.Value.Regex ?? DefaultSettings.DefaultResxRegex);

        await Parallel.ForEachAsync(await handle.ValuesAsync(), async (entry, token) =>
        {
            if (token.IsCancellationRequested) return;

            var match = regex.Match(entry.Name);
            var currentPath = directoryPath + Path.DirectorySeparatorChar + entry.Name;
            switch (entry.Kind)
            {
                case FileSystemHandleKind.File when match.Success:
                    var name = match.Groups[DefaultSettings.ResourceResxName].Value;
                    var code = match.Groups[DefaultSettings.ResourceResxCode].Value;
                    var resource = new Resource(new CultureInfo(code), await handle.GetFileHandleAsync(entry.Name));
                    resources.AddOrUpdate(name, [resource], (_, list) => [..list, resource]);
                    break;
                case FileSystemHandleKind.File:
                    // Non-resource file
                    break;
                case FileSystemHandleKind.Directory:
                    await using (var directory = await handle.GetDirectoryHandleAsync(entry.Name))
                    {
                        var childNodes = await GetTreeItems(currentPath, directory);
                        if (childNodes.Count <= 0) return;

                        items.Add(new TreeViewItem
                        {
                            Text = currentPath,
                            Items = childNodes,
                            IconCollapsed = new Icons.Regular.Size20.Folder(),
                            IconExpanded = new Icons.Regular.Size20.FolderOpen()
                        });
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(handle));
            }
        });

        foreach (var resource in resources)
        {
            items.Add(new TreeViewItem
            {
                Text = directoryPath + Path.DirectorySeparatorChar + resource.Key,
                IconCollapsed = new Icons.Regular.Size20.BookLetter()
            });

            dispatcher.Dispatch(new AddResourceAction(new ResxFile
            {
                Path = directoryPath,
                Name = resource.Key,
                Cultures = resource.Value.Select(e => e.Culture),
                Handles = resource.Value.ToDictionary(e => e.Culture, e => e.Handle)
            }));
        }

        return items;
    }
}