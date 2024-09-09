using Microsoft.Extensions.DependencyInjection;
using ResxManagerExtended.Desktop.Services;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Services;

namespace ResxManagerExtended.Desktop;

internal static class Bootstrapper
{
    internal static void ConfigureServices(this IServiceCollection collection, bool isDevelopment)
    {
        collection.AddScoped<IResourceService, ResourceService>();
        collection.AddScoped<ISettingService, SettingService>();

        collection.ConfigureNecessaries(isDevelopment);
    }
}