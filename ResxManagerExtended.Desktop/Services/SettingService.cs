using System.Configuration;
using ResxManagerExtended.Desktop.Properties;
using ResxManagerExtended.Shared.Services;

namespace ResxManagerExtended.Desktop.Services;

internal class SettingService : ISettingService
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
}