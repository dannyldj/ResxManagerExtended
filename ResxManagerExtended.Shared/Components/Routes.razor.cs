using Fluxor;
using Microsoft.AspNetCore.Components;
using ResxManagerExtended.Shared.Store.Settings;

namespace ResxManagerExtended.Shared.Components;

public partial class Routes
{
    [Inject] public required IDispatcher Dispatcher { private get; init; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new GetRegexAction());
    }
}