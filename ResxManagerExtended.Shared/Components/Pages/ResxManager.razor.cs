using System.Globalization;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Comparer;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Store.ResxManager.UseCase;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class ResxManager : FluxorComponent
{
    private SortedSet<CultureInfo> _cultures = [];
    private bool _isLoading = true;
    private IQueryable<ResourceView> _items = Enumerable.Empty<ResourceView>().AsQueryable();
    private bool _showPath;

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [Inject] public required IState<ResxManagerState> ResxManagerState { private get; init; }

    [Inject] public required IState<SettingState> SettingState { private get; init; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _items = await GetDataGrid();
        _isLoading = false;
    }

    private async Task OnSelectedItemChanged(ITreeViewItem? item)
    {
        _isLoading = true;
        _items = await GetDataGrid(item?.Id);
        _isLoading = false;
    }

    private async Task<IQueryable<ResourceView>> GetDataGrid(string? selectedPath = null)
    {
        var resources = new List<ResourceView>();
        _cultures = new SortedSet<CultureInfo>(new CultureComparer());

        foreach (var valueResource in ResxManagerState.Value.Resources ?? [])
        {
            if (selectedPath is not null &&
                $"{valueResource.Path}{Path.DirectorySeparatorChar}{valueResource.Name}".IsUnderDirectory(selectedPath)
                    is false) continue;

            resources.AddRange(await valueResource.GetValues());
            foreach (var culture in valueResource.Cultures ?? [])
            {
                _cultures.Add(culture);
            }
        }

        return resources.AsQueryable();
    }
}