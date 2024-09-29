using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using Fluxor;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Services;
using ResxManagerExtended.Shared.Store;
using ResxManagerExtended.Shared.Store.UseCase;
using ResxManagerExtended.Web.Data;

namespace ResxManagerExtended.Web.Services;

internal class ResourceService(
    IFileSystemAccessServiceInProcess fileSystemAccessService,
    IDispatcher dispatcher,
    IState<ResourceState> resourceState) : IResourceService
{
    private readonly FilePickerAcceptType _csvAcceptType = new()
        { Accept = new Dictionary<string, string[]> { { "text/csv", [".csv"] } }, Description = "CSV File" };

    private readonly List<ResxFile> _resxFiles = [];

    public async Task<IEnumerable<ITreeViewItem>?> SetNodes()
    {
        try
        {
            await using var handle = await fileSystemAccessService.ShowDirectoryPickerAsync();
            var root = new TreeViewItem
            {
                Text = handle.Name,
                Items = await GetTreeItems(handle.Name, handle),
                IconCollapsed = new Icons.Regular.Size20.Folder(),
                IconExpanded = new Icons.Regular.Size20.FolderOpen(),
                Expanded = true
            };

            dispatcher.Dispatch(new SetResourcesAction(
                _resxFiles.ToDictionary<ResxFile, string, IResourceFile>(e => e.GetFullPath(), e => e)));

            return [root];
        }
        catch (JSException)
        {
            // Closing the DirectoryPicker throws an exception.
            return null;
        }
    }

    public async IAsyncEnumerable<ResourceView>? ImportResources()
    {
        FileSystemFileHandleInProcess handle;

        try
        {
            var selectedFiles =
                await fileSystemAccessService.ShowOpenFilePickerAsync(new OpenFilePickerOptionsStartInFileSystemHandle
                    { Types = [_csvAcceptType] });

            handle = selectedFiles.Single();
        }
        catch (JSException)
        {
            // Closing the OpenFilePicker throws an exception.
            yield break;
        }

        await using var file = await handle.GetFileAsync();
        using var reader = new StringReader(await file.TextAsync());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<ResourceViewMap>();
        await foreach (var resource in csv.GetRecordsAsync<ResourceView>())
        {
            yield return resource;
        }
    }

    public async Task ExportResources(IReadOnlyList<CultureInfo> cultures, IEnumerable<ResourceView> resources,
        CancellationToken token)
    {
        try
        {
            await using var handle =
                await fileSystemAccessService.ShowSaveFilePickerAsync(new SaveFilePickerOptionsStartInFileSystemHandle
                    { Types = [_csvAcceptType] });

            await using var writable = await handle.CreateWritableAsync();
            await using var writer = new StreamWriter(writable, Encoding.UTF8);
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.ExportCsvAsync(cultures, resources);
        }
        catch (JSException)
        {
            // Closing the SaveFilePicker throws an exception.
        }
    }

    private async Task<List<ITreeViewItem>> GetTreeItems(string directoryPath,
        FileSystemDirectoryHandleInProcess handle)
    {
        var items = new List<ITreeViewItem>();
        var resources = new ConcurrentDictionary<string, IEnumerable<Resource>>();
        var regex = new Regex(resourceState.Value.Regex ?? DefaultSettings.DefaultResxRegex);

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

            _resxFiles.Add(new ResxFile
            {
                Path = directoryPath,
                Name = resource.Key,
                Cultures = resource.Value.Select(e => e.Culture),
                Handles = resource.Value.ToDictionary(e => e.Culture, e => e.Handle)
            });
        }

        return items;
    }

    private record Resource(CultureInfo Culture, FileSystemFileHandleInProcess Handle);
}