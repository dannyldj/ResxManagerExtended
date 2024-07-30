using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using ResxManagerExtended.Shared.Store.Settings;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class Settings : FluxorComponent
{
    [Inject] public required IDispatcher Dispatcher { get; set; }
    
    [Inject] public required IState<RootDirectoryState> DirectoryState { get; set; }

    private void GetRootDirectory()
    {
        Dispatcher.Dispatch(new GetRootAction());
    }
}