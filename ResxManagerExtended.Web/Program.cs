using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ResxManagerExtended.Shared.Components;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.ConfigureServices(builder.HostEnvironment.IsDevelopment());

var host = builder.Build();

var localStorageService = host.Services.GetService<ILocalStorageService>();
if (localStorageService is not null)
{
    var result = await localStorageService.GetItemAsStringAsync(SettingKeys.CultureKey);
    var culture = result is null ? DefaultSettings.DefaultCulture : CultureInfo.GetCultureInfo(result);

    if (result is null)
        await localStorageService.SetItemAsStringAsync(SettingKeys.CultureKey,
            DefaultSettings.DefaultCulture.Name);

    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

await host.RunAsync();