using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Fluxor;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Win32;
using ResxManagerExtended.Desktop.Data;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Services;
using ResxManagerExtended.Shared.Store.ResxManager;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Desktop.Services;

internal class ResourceService(IDispatcher dispatcher, IState<SettingState> settingState) : IResourceService
{
    public Task<ITreeViewItem?> SetTopNode()
    {
        var dialog = new OpenFolderDialog();
        if (dialog.ShowDialog() is not true)
            return Task.FromResult<ITreeViewItem?>(null);

        dispatcher.Dispatch(new SetResourcesAction(null));

        return Task.FromResult<ITreeViewItem?>(new TreeViewItem
        {
            Text = dialog.FolderName,
            Items = GetTreeItems(dialog.FolderName),
            IconCollapsed = new Icons.Regular.Size20.Folder(),
            IconExpanded = new Icons.Regular.Size20.FolderOpen(),
            Expanded = true
        });
    }

    private List<ITreeViewItem> GetTreeItems(string directoryPath)
    {
        var resources = new ConcurrentDictionary<string, IEnumerable<CultureInfo>>();
        var regex = new Regex(settingState.Value.Regex ?? DefaultSettings.DefaultResxRegex);
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

            dispatcher.Dispatch(new AddResourceAction(new ResxFile
            {
                Path = directoryPath,
                Name = resource.Key,
                Cultures = resource.Value
            }));
        }

        return items;
    }
}