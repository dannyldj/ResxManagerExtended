using System.Globalization;
using Blazored.LocalStorage;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ResxManagerExtended.Shared.Components;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Routes).Assembly).UseRouting();

    if (builder.HostEnvironment.IsDevelopment())
        options.UseReduxDevTools(middlewareOptions => { middlewareOptions.Name = "ResxManagerExtended"; });
});

builder.Services.ConfigureServices();

var host = builder.Build();

var localStorageService = host.Services.GetService<ILocalStorageService>();
if (localStorageService is not null)
{
    var result = await localStorageService.GetItemAsStringAsync(LocalStorageKeys.CultureKey);
    var culture = result is null ? DefaultSettings.DefaultCulture : CultureInfo.GetCultureInfo(result);

    if (result is null)
        await localStorageService.SetItemAsStringAsync(LocalStorageKeys.CultureKey,
            DefaultSettings.DefaultCulture.Name);

    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

await host.RunAsync();