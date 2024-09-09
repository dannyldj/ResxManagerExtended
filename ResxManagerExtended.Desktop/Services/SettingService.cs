using ResxManagerExtended.Desktop.Properties;
using ResxManagerExtended.Shared.Services;

namespace ResxManagerExtended.Desktop.Services;

internal class SettingService : ISettingService
{
    public Task<string?> GetOptionAsStringAsync(string key)
    {
        return Task.FromResult(Settings.Default[key]?.ToString());
    }

    public Task SetOptionAsStringAsync(string key, string value)
    {
        if (Settings.Default[key] is null)
            // Not support setting key
            return Task.CompletedTask;

        Settings.Default[key] = value;
        Settings.Default.Save();
        return Task.CompletedTask;
    }
}