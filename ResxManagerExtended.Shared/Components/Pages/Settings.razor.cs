using System.Globalization;
using Blazored.LocalStorage;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Store.Settings;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class Settings : FluxorComponent
{
    private bool _isReadOnlyResourceRegex = true;
    private CultureInfo? _selectedCulture;

    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }
    [Inject] public required IDialogService DialogService { private get; init; }
    [Inject] public required IDispatcher Dispatcher { private get; init; }
    [Inject] public required ILocalStorageService LocalStorageService { private get; init; }
    [Inject] public required NavigationManager NavigationManager { private get; init; }
    [Inject] public required IState<SettingState> SettingState { private get; init; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _selectedCulture = CultureInfo.CurrentCulture;
    }

    private async Task CultureChanged()
    {
        if (_selectedCulture is null || Equals(CultureInfo.CurrentCulture, _selectedCulture)) return;

        await LocalStorageService.SetItemAsStringAsync(LocalStorageKeys.CultureKey, _selectedCulture.Name);
        NavigationManager.NavigateTo(NavigationManager.Uri, true);
    }

    private void GetRootDirectory()
    {
        Dispatcher.Dispatch(new GetRootAction());
    }

    private void ResourceRegexChanged(string? value)
    {
        Dispatcher.Dispatch(new SetRegexAction(value));
    }

    private void OnClickResourceRegex(MouseEventArgs args)
    {
        if (_isReadOnlyResourceRegex == false) return;

        DialogService.ShowMessageBox(new DialogParameters<MessageBoxContent>
        {
            Content = new MessageBoxContent
            {
                Title = Loc["Warning"],
                Intent = MessageBoxIntent.Warning,
                Icon = new Icons.Filled.Size24.Warning(),
                IconColor = Color.Warning,
                Message = Loc["ConfirmEditSettings"]
            },
            PrimaryAction = Loc["Yes"],
            SecondaryAction = Loc["No"],
            Modal = true,
            OnDialogResult = EventCallback.Factory.Create(this, (Func<DialogResult, Task>)OnWarningDialogResult)
        });
    }

    private Task OnWarningDialogResult(DialogResult result)
    {
        _isReadOnlyResourceRegex = result.Cancelled;

        return Task.CompletedTask;
    }
}