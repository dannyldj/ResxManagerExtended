using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using Fluxor;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Win32;
using ResxManagerExtended.Desktop.Data;
using ResxManagerExtended.Desktop.Extensions;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Services;
using ResxManagerExtended.Shared.Store;
using ResxManagerExtended.Shared.Store.UseCase;

namespace ResxManagerExtended.Desktop.Services;

internal class ResourceService(IDispatcher dispatcher, IState<ResourceState> resourceState) : IResourceService
{
    private readonly List<ResxFile> _resxFiles = [];

    public async Task<ITreeViewItem?> SetTopNode()
    {
        var dialog = new OpenFolderDialog();
        if (await dialog.ShowDialogAsync() is not true)
            return null;

        var root = new TreeViewItem
        {
            Text = dialog.FolderName,
            Items = GetTreeItems(dialog.FolderName),
            IconCollapsed = new Icons.Regular.Size20.Folder(),
            IconExpanded = new Icons.Regular.Size20.FolderOpen(),
            Expanded = true
        };
        _resxFiles.ForEach(file => file.RelativePath = file.GetRelativePath(dialog.FolderName));

        dispatcher.Dispatch(new SetResourcesAction(_resxFiles));

        return root;
    }

    public async Task ExportResources(ImmutableArray<CultureInfo> cultures, IEnumerable<ResourceView> resources,
        CancellationToken token)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "CSV File|*.csv"
        };
        if (await dialog.ShowDialogAsync() is not true) return;

        await using var writer = new StreamWriter(dialog.FileName, true, Encoding.UTF8);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.ExportCsv(cultures, resources);
    }

    private List<ITreeViewItem> GetTreeItems(string directoryPath)
    {
        var resources = new ConcurrentDictionary<string, IEnumerable<CultureInfo>>();
        var regex = new Regex(resourceState.Value.Regex ?? DefaultSettings.DefaultResxRegex);
        var items = (from dir in Directory.GetDirectories(directoryPath)
                let childNodes = GetTreeItems(dir)
                where childNodes.Count > 0
                select new TreeViewItem
                {
                    Text = dir,
                    Items = childNodes,
                    IconCollapsed = new Icons.Regular.Size20.Folder(),
                    IconExpanded = new Icons.Regular.Size20.FolderOpen()
                })
            .Cast<ITreeViewItem>().ToList();

        Parallel.ForEach(Directory.GetFiles(directoryPath), file =>
        {
            var match = regex.Match(Path.GetFileName(file));
            if (match.Success is false) return;

            var name = match.Groups[DefaultSettings.ResourceResxName].Value;
            var code = match.Groups[DefaultSettings.ResourceResxCode].Value;

            resources.AddOrUpdate(name, [new CultureInfo(code)], (_, list) => [..list, new CultureInfo(code)]);
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
                Cultures = resource.Value
            });
        }

        return items;
    }
}