using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Fluxor;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Win32;
using ResxManagerExtended.Desktop.Data;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Services;
using ResxManagerExtended.Shared.Store.ResxManager;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Desktop.Services;

internal class ResourceService(IDispatcher dispatcher, IState<SettingState> settingState) : IResourceService
{
    public Task<ITreeViewItem?> SetTopNode()
    {
        var dialog = new OpenFolderDialog();
        var directoryPath = string.Empty;
        if (dialog.ShowDialog() is true) directoryPath = dialog.FolderName;

        dispatcher.Dispatch(new SetResourcesAction(null));

        var root = new TreeViewItem
        {
            Id = directoryPath,
            Text = Path.GetFileName(directoryPath),
            Items = GetTreeItems(directoryPath),
            Expanded = true
        };

        return Task.FromResult<ITreeViewItem?>(root);
    }

    private List<ITreeViewItem> GetTreeItems(string directoryPath)
    {
        var resources = new ConcurrentDictionary<string, List<CultureInfo>>();
        var regex = new Regex(settingState.Value.Regex ?? DefaultSettings.DefaultResxRegex);
        var items = (from dir in Directory.GetDirectories(directoryPath)
                let childNodes = GetTreeItems(dir)
                where childNodes.Count > 0
                select new TreeViewItem
                    { Id = dir, Text = Path.GetFileName(dir), Items = childNodes, Expanded = false })
            .Cast<ITreeViewItem>().ToList();

        Parallel.ForEach(Directory.GetFiles(directoryPath), file =>
        {
            var match = regex.Match(Path.GetFileName(file));
            if (match.Success is false) return;

            var name = match.Groups[DefaultSettings.ResourceResxName].Value;
            var code = match.Groups[DefaultSettings.ResourceResxCode].Value;

            if (resources.TryGetValue(name, out var codes))
                codes.Add(new CultureInfo(code));
            else
                resources.AddOrUpdate(name, _ => [new CultureInfo(code)], (_, list) => [..list, new CultureInfo(code)]);
        });

        foreach (var resource in resources)
        {
            items.Add(new TreeViewItem
            {
                Id = directoryPath + resource + ResxExtension.FileExtension,
                Text = resource.Key
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