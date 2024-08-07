using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ResxManagerExtended.Shared.Properties;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class ResxManager
{
    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }
}