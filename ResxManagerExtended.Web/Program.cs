using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ResxManagerExtended.Shared.Components;
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

await builder.Build().RunAsync();