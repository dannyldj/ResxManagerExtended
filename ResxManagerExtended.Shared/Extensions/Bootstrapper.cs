using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Components;
using ResxManagerExtended.Shared.Interfaces;
using ResxManagerExtended.Shared.Services;

namespace ResxManagerExtended.Shared.Extensions;

public static class Bootstrapper
{
    public static void ConfigureServices(this IServiceCollection collection, bool isDevelopment)
    {
        collection.AddLocalization();
        collection.AddFluentUIComponents();
        collection.AddFluxor(options =>
        {
            options.ScanAssemblies(typeof(Routes).Assembly).UseRouting();

            if (isDevelopment)
                options.UseReduxDevTools(middlewareOptions => { middlewareOptions.Name = "ResxManagerExtended"; });
        });

        if (OperatingSystem.IsBrowser())
            collection.AddScoped<IFileSystemAccessService, FileSystemAccessServiceByApi>();
        else
        {
            // Todo: Writing service that work in environments other than WASM
            // https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-javascript-from-dotnet?view=aspnetcore-8.0
        }
    }
}