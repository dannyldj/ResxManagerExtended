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
using ResxManagerExtended.Shared.Store;
using ResxManagerExtended.Shared.Store.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class ResxManager : FluxorComponent
{
    private SortedSet<CultureInfo> _cultures = [];
    private bool _isLoading = true;
    private IEnumerable<ResourceView> _items = [];
    private PopoverType _popoverType;
    private string? _searchValue;
    private bool _showPath, _showComment, _showPopover;

    private IQueryable<ResourceView> SearchedItems => string.IsNullOrEmpty(_searchValue)
        ? _items.AsQueryable()
        : _items.Where(item =>
                item.Key.Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
                item.Columns.Any(e =>
                    e.Value != null && e.Value.Contains(_searchValue, StringComparison.OrdinalIgnoreCase)))
            .AsQueryable();

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }
    [Inject] public required IDispatcher Dispatcher { private get; init; }
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

    private void OnConfirm()
    {
        switch (_popoverType)
        {
            case PopoverType.Import:
                Dispatcher.Dispatch(new ImportAction());
                break;
            case PopoverType.Export:
                Dispatcher.Dispatch(new ExportAction([.._cultures], _items));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task GetDataGrid(ITreeViewItem? selectedNode = null)
    {
        _isLoading = true;
        _items = [];
        _cultures = new SortedSet<CultureInfo>(new CultureComparer());

        if (ResourceState.Value.Resources is not null)
            foreach (var resource in ResourceState.Value.Resources)
            {
                if (selectedNode is not null &&
                    resource.GetFullPath().IsUnderDirectory(selectedNode.Text) is false) continue;

                _items = [.._items, ..await resource.GetValues()];
                foreach (var culture in resource.Cultures ?? [])
                {
                    _cultures.Add(culture);
                }
            }

        _searchValue = string.Empty;
        _isLoading = false;
    }

    private enum PopoverType
    {
        Import,
        Export
    }
}