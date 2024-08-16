using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class ResxManager : FluxorComponent
{
    private IEnumerable<Resource>? _handles;
    private IEnumerable<Resource>? _selectedHandles;

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [Inject] public required IState<RootDirectoryState> DirectoryState { private get; init; }

    protected override async Task OnInitializedAsync()
    {
        if (DirectoryState.Value.DirectoryHandle is null) return;

        _handles = await DirectoryState.Value.DirectoryHandle.GetResourceFiles();
    }
}