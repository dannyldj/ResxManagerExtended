using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Components;

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

        collection.AddFileSystemAccessServiceInProcess();
    }
}