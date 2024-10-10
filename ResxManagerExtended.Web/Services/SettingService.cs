using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ResxManagerExtended.Shared.Services;

namespace ResxManagerExtended.Web.Services;

internal class SettingService(
    ILocalStorageService localStorageService,
    NavigationManager navigationManager) : ISettingService
{
    public async Task<string?> GetOptionAsStringAsync(string key)
    {
        return await localStorageService.GetItemAsStringAsync(key);
    }

    public async Task SetOptionAsStringAsync(string key, string value)
    {
        await localStorageService.SetItemAsStringAsync(key, value);
    }

    public void ReloadApp()
    {
        navigationManager.NavigateTo(navigationManager.Uri, true);
    }
}