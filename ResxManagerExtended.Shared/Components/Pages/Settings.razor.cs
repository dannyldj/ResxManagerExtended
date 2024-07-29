using Fluxor;
using Microsoft.AspNetCore.Components;
using ResxManagerExtended.Shared.Store.Settings;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class Settings
{
    [Inject] public required IDispatcher Dispatcher { get; init; }

    private void GetRootDirectory()
    {
        Dispatcher.Dispatch(new GetRootAction());
    }
}