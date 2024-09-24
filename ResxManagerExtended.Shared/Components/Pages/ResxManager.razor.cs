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
using ResxManagerExtended.Shared.Store.UseCase;

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

    [Inject] public required IState<ResourceState> ResourceState { private get; init; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetDataGrid();

        ResourceState.StateChanged += ResourceStateOnStateChanged;
    }

    private async void ResourceStateOnStateChanged(object? sender, EventArgs e)
    {
        await GetDataGrid();
        StateHasChanged();
    }

    private async Task GetDataGrid(ITreeViewItem? selectedNode = null)
    {
        _isLoading = true;
        _items = [];
        _cultures = new SortedSet<CultureInfo>(new CultureComparer());

        foreach (var valueResource in ResourceState.Value.Resources ?? [])
        {
            if (selectedNode is not null &&
                valueResource.GetFullPath().IsUnderDirectory(selectedNode.Text) is false) continue;

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