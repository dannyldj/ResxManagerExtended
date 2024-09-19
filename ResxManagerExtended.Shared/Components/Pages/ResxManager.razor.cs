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
    private IEnumerable<ResourceView> _items = [];
    private string? _searchValue;
    private bool _showPath, _showComment;

    private IQueryable<ResourceView> SearchedItems => string.IsNullOrEmpty(_searchValue)
        ? _items.AsQueryable()
        : _items.Where(item =>
                item.Key.Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
                item.Columns.Any(e =>
                    e.Value != null && e.Value.Contains(_searchValue, StringComparison.OrdinalIgnoreCase)))
            .AsQueryable();

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [Inject] public required IState<ResxManagerState> ResxManagerState { private get; init; }

    [Inject] public required IState<SettingState> SettingState { private get; init; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetDataGrid();

        SettingState.StateChanged += SettingStateOnStateChanged;
    }

    private async void SettingStateOnStateChanged(object? sender, EventArgs e)
    {
        await GetDataGrid();
        StateHasChanged();
    }

    private async Task OnSelectedItemChanged(ITreeViewItem? item)
    {
        await GetDataGrid(item?.Id);
    }

    private async Task GetDataGrid(string? selectedPath = null)
    {
        _cultures = new SortedSet<CultureInfo>(new CultureComparer());
        _isLoading = true;

        foreach (var valueResource in ResxManagerState.Value.Resources ?? [])
        {
            if (selectedPath is not null &&
                $"{valueResource.Path}{Path.DirectorySeparatorChar}{valueResource.Name}".IsUnderDirectory(selectedPath)
                    is false) continue;

            _items = [.._items, ..await valueResource.GetValues()];
            foreach (var culture in valueResource.Cultures ?? [])
            {
                _cultures.Add(culture);
            }
        }

        _searchValue = string.Empty;
        _isLoading = false;
    }
}