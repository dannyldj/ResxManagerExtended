using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class ResxManager : FluxorComponent
{
    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [Inject] public required IState<SettingState> SettingState { private get; init; }
}