using System.Globalization;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Store.ResxManager.UseCase;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class ResxManager : FluxorComponent
{
    private HashSet<CultureInfo> _cultures = [];
    private FluentDataGrid<ResourceView>? _grid;
    private IQueryable<ResourceView> _items = Enumerable.Empty<ResourceView>().AsQueryable();

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [Inject] public required IState<ResxManagerState> ResxManagerState { private get; init; }

    [Inject] public required IState<SettingState> SettingState { private get; init; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _items = await GetDataGrid();
    }

    private async void OnSelectedItemChanged(ITreeViewItem? item)
    {
        _items = await GetDataGrid(item?.Id);
        _grid?.SetLoadingState(false);
    }

    private async Task<IQueryable<ResourceView>> GetDataGrid(string? selectedPath = null)
    {
        var resources = new List<ResourceView>();
        _cultures = [];

        _grid?.SetLoadingState(true);
        foreach (var valueResource in ResxManagerState.Value.Resources ?? [])
        {
            if (selectedPath is not null && valueResource.Path.IsUnderDirectory(selectedPath) is false) continue;

            resources.AddRange(await valueResource.GetValues());
            foreach (var culture in valueResource.Cultures ?? [])
            {
                _cultures.Add(culture);
            }
        }

        return resources.AsQueryable();
    }
}