using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ResxManagerExtended.Shared.Properties;

namespace ResxManagerExtended.Shared.Components.Layout;

public partial class MainLayout
{
    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }
}