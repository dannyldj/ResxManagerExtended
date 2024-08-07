using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Store.Settings;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class Settings : FluxorComponent
{
    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [Inject] public required IDispatcher Dispatcher { private get; init; }

    [Inject] public required IState<RootDirectoryState> DirectoryState { private get; init; }

    private void GetRootDirectory()
    {
        Dispatcher.Dispatch(new GetRootAction());
    }
}