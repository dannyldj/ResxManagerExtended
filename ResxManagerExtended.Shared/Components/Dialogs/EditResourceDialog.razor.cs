using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Properties;

namespace ResxManagerExtended.Shared.Components.Dialogs;

public partial class EditResourceDialog
{
    private CultureInfo _selectedCulture = CultureInfo.InvariantCulture;
    private string? _inputValue;

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [CascadingParameter] public required FluentDialog Dialog { get; set; }
    [Parameter] public required ResourceView Content { get; set; }

    private async Task SaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}