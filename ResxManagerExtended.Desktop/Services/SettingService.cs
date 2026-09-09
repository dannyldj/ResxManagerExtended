using System.Configuration;
using System.Diagnostics;
using System.Windows;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Desktop.Properties;
using ResxManagerExtended.Shared.Properties;
using ResxManagerExtended.Shared.Services;

namespace ResxManagerExtended.Desktop.Services;

internal class SettingService(IDialogService dialogService) : ISettingService
{
    public Task<string?> GetOptionAsStringAsync(string key)
    {
        var value = Settings.Default[key].ToString();
        return string.IsNullOrEmpty(value) ? Task.FromResult<string?>(null) : Task.FromResult<string?>(value);
    }

    public Task SetOptionAsStringAsync(string key, string value)
    {
        try
        {
            Settings.Default[key] = value;
            Settings.Default.Save();
            return Task.CompletedTask;
        }
        catch (SettingsPropertyNotFoundException ex)
        {
            // Not support setting key
            return Task.FromException(ex);
        }
    }

    public void ReloadApp()
    {
        dialogService.ShowConfirmation(this, async result =>
        {
            if (result.Cancelled) return;

            await Task.Yield();
            Process.Start(Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty);
            Application.Current.Shutdown();
        }, Resources.RestartConfirm, Resources.Yes, Resources.No);
    }
}