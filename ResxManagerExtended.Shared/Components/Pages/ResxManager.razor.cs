using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class ResxManager : FluxorComponent
{
    private IEnumerable<ITreeViewItem>? _resourceTree;

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [Inject] public required IState<SettingState> SettingState { private get; init; }

    protected override async Task OnInitializedAsync()
    {
        if (SettingState.Value.DirectoryHandle is null) return;

        _resourceTree = await SettingState.Value.DirectoryHandle.GetResourceFiles();
    }
}