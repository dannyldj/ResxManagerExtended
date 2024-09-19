using Blazored.LocalStorage;
using ResxManagerExtended.Shared.Services;

namespace ResxManagerExtended.Web.Services;

internal class SettingService(ILocalStorageService localStorageService) : ISettingService
{
    public async Task<string?> GetOptionAsStringAsync(string key)
    {
        return await localStorageService.GetItemAsStringAsync(key);
    }

    public async Task SetOptionAsStringAsync(string key, string value)
    {
        await localStorageService.SetItemAsStringAsync(key, value);
    }
}