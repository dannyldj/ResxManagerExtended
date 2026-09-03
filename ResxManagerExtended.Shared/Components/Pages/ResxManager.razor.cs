using System.Globalization;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Comparer;
using ResxManagerExtended.Shared.Components.Dialogs;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Store;
using ResxManagerExtended.Shared.Store.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class ResxManager : FluxorComponent
{
    private readonly HashSet<ResourceView> _selectedItems = [];
    private SortedSet<CultureInfo> _cultures = [];
    private bool _isLoading = true;
    private IEnumerable<ResourceView> _items = [];
    private string? _searchValue;
    private bool? _selectAll = false;
    private ITreeViewItem? _selectedNode;
    private bool _showPath, _showComment;

    private IQueryable<ResourceView> SearchedItems => string.IsNullOrEmpty(_searchValue)
        ? _items.AsQueryable()
        : _items.Where(item =>
                item.Key.Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
                item.Columns.Any(e =>
                    e.Value != null && e.Value.Contains(_searchValue, StringComparison.OrdinalIgnoreCase)))
            .AsQueryable();

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }
    [Inject] public required IDispatcher Dispatcher { private get; init; }
    [Inject] public required IDialogService DialogService { private get; init; }
    [Inject] public required IState<ResourceState> ResourceState { private get; init; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetDataGrid();

        ResourceState.StateChanged += ResourceStateOnStateChanged;
    }

    private async void ResourceStateOnStateChanged(object? sender, EventArgs e)
    {
        try
        {
            // 리소스 파일을 쓰는 중에는 다시 읽지 않고, 처리가 끝난 뒤에 갱신한다.
            if (ResourceState.Value.IsResourceProcessing)
            {
                return;
            }

            await GetDataGrid();
            StateHasChanged();
        }
        catch (Exception)
        {
            // TODO: Logger 구성 후 예외 기록 및 사용자 알림 추가
        }
    }

    private async Task OnRowDoubleClick(FluentDataGridRow<ResourceView> obj)
    {
        if (obj.Item is null)
        {
            return;
        }

        var dialog = await DialogService.ShowDialogAsync<EditResourceDialog>(obj.Item,
            new DialogParameters { PreventDismissOnOverlayClick = true, PreventScroll = true });
        var result = await dialog.Result;

        if (result.Cancelled || result.Data is not EditResourceResult edit)
        {
            return;
        }

        Dispatcher.Dispatch(new EditResourceAction(obj.Item, edit.Culture, edit.Value));
    }

    private void ToggleSelection(ResourceView item, bool selected)
    {
        if (selected)
        {
            _selectedItems.Add(item);
        }
        else
        {
            _selectedItems.Remove(item);
        }

        RefreshSelectAllState();
    }

    private void SelectAllChanged(bool? selected)
    {
        // 전체 선택은 현재 검색 필터가 적용된 목록만 대상으로 한다.
        _selectedItems.Clear();

        if (selected is true)
        {
            foreach (var item in SearchedItems)
            {
                _selectedItems.Add(item);
            }
        }

        RefreshSelectAllState();
    }

    private void RefreshSelectAllState()
    {
        if (_selectedItems.Count == 0)
        {
            _selectAll = false;
            return;
        }

        var filtered = SearchedItems.ToList();
        _selectAll = filtered.Count > 0 && filtered.TrueForAll(_selectedItems.Contains) ? true : null;
    }

    private async Task DeleteSelectedAsync()
    {
        if (_selectedItems.Count == 0)
        {
            return;
        }

        var dialog = await DialogService.ShowConfirmationAsync(
            Loc["ConfirmDeleteResources", _selectedItems.Count], Loc["Yes"], Loc["No"], Loc["Warning"]);
        var result = await dialog.Result;

        if (result.Cancelled)
        {
            return;
        }

        Dispatcher.Dispatch(new DeleteResourcesAction([.._selectedItems]));
    }

    private async Task SelectNode(ITreeViewItem? selectedNode)
    {
        _selectedNode = selectedNode;
        _searchValue = string.Empty;

        await GetDataGrid();
    }

    private async Task GetDataGrid()
    {
        _isLoading = true;
        _items = [];
        _cultures = new SortedSet<CultureInfo>(new CultureComparer());

        // 항목을 다시 읽으면 이전 인스턴스를 가리키던 선택은 의미가 없다.
        _selectedItems.Clear();
        _selectAll = false;

        try
        {
            if (ResourceState.Value.Resources is not null)
            {
                foreach (var resource in ResourceState.Value.Resources)
                {
                    if (_selectedNode is not null && !resource.GetFullPath().IsUnderDirectory(_selectedNode.Text))
                    {
                        continue;
                    }

                    _items = [.. _items, .. await resource.GetValues()];
                    foreach (var culture in resource.Cultures ?? [])
                    {
                        _cultures.Add(culture);
                    }
                }
            }
        }
        finally
        {
            _isLoading = false;
        }
    }
}