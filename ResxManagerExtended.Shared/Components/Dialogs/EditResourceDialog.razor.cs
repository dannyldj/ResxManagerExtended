using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Data;
using ResxManagerExtended.Shared.Properties;

namespace ResxManagerExtended.Shared.Components.Dialogs;

public partial class EditResourceDialog
{
    private string? _inputValue;
    private string? _originalValue;
    private CultureInfo _selectedCulture = CultureInfo.InvariantCulture;

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [CascadingParameter] public required FluentDialog Dialog { get; set; }

    private string? NeutralValue => Content.Columns.GetValueOrDefault(CultureInfo.InvariantCulture);

    private bool IsUnchanged =>
        string.Equals(_inputValue ?? string.Empty, _originalValue ?? string.Empty, StringComparison.Ordinal);

    [Parameter] public required ResourceView Content { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (!Content.Columns.ContainsKey(_selectedCulture))
        {
            _selectedCulture = Content.Columns.Keys.FirstOrDefault() ?? CultureInfo.InvariantCulture;
        }

        LoadValue();
    }

    private void OnCultureChanged(CultureInfo? culture)
    {
        if (culture is null)
        {
            return;
        }

        _selectedCulture = culture;
        LoadValue();
    }

    private void LoadValue()
    {
        _originalValue = Content.Columns.GetValueOrDefault(_selectedCulture);
        _inputValue = _originalValue;
    }

    private async Task SaveAsync()
    {
        await Dialog.CloseAsync(new EditResourceResult(_selectedCulture, _inputValue ?? string.Empty));
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}