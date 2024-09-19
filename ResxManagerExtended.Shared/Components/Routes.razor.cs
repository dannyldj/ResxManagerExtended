using Fluxor;
using Microsoft.AspNetCore.Components;
using ResxManagerExtended.Shared.Components.Pages;

namespace ResxManagerExtended.Shared.Components;

public partial class Routes
{
    [Inject] public required IDispatcher Dispatcher { private get; init; }

    protected override void OnInitialized()
    {
        Settings.InitializeSettings(Dispatcher);
    }
}