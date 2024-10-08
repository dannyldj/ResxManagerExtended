﻿using System.Collections.Concurrent;
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
    private const string CsvFilter = "CSV File|*.csv";

    public async Task<IEnumerable<ITreeViewItem>?> SetNodes()
    {
        var dialog = new OpenFolderDialog();
        if (await dialog.ShowDialogAsync() is not true)
            return null;

        var resxFiles = new List<ResxFile>();
        var root = new TreeViewItem
        {
            Text = dialog.FolderName,
            Items = GetTreeItems(dialog.FolderName, resxFiles),
            IconCollapsed = new Icons.Regular.Size20.Folder(),
            IconExpanded = new Icons.Regular.Size20.FolderOpen(),
            Expanded = true
        };
        resxFiles.ForEach(file => file.RelativePath = file.GetRelativePath(dialog.FolderName));

        dispatcher.Dispatch(new SetResourcesAction(resxFiles));

        return [root];
    }

    public async IAsyncEnumerable<ResourceView>? ImportResources()
    {
        var dialog = new OpenFileDialog { Filter = CsvFilter };
        if (await dialog.ShowDialogAsync() is not true) yield break;

        using var reader = new StreamReader(dialog.FileName);
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
        var dialog = new SaveFileDialog { Filter = CsvFilter };
        if (await dialog.ShowDialogAsync() is not true) return;

        await using var writer = new StreamWriter(dialog.FileName, false, Encoding.UTF8);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        await csv.ExportCsvAsync(cultures, resources);
    }

    private List<ITreeViewItem> GetTreeItems(string directoryPath, List<ResxFile> resxFiles)
    {
        var resources = new ConcurrentDictionary<string, IEnumerable<Resource>>();
        var regex = new Regex(resourceState.Value.Regex ?? DefaultSettings.DefaultResxRegex);
        var items = (from dir in Directory.GetDirectories(directoryPath)
                let childNodes = GetTreeItems(dir, resxFiles)
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

            resources.AddOrUpdate(name, [new Resource(new CultureInfo(code), file)],
                (_, list) => [..list, new Resource(new CultureInfo(code), file)]);
        });

        foreach (var resource in resources)
        {
            items.Add(new TreeViewItem
            {
                Text = directoryPath + Path.DirectorySeparatorChar + resource.Key,
                IconCollapsed = new Icons.Regular.Size20.BookLetter()
            });

            resxFiles.Add(new ResxFile
            {
                Path = directoryPath,
                Name = resource.Key,
                Cultures = resource.Value.Select(e => e.Culture),
                Paths = resource.Value.ToDictionary(e => e.Culture, e => e.Path)
            });
        }

        return items;
    }

    private record Resource(CultureInfo Culture, string Path);
}