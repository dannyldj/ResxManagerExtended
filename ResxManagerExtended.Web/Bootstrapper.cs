using Blazored.LocalStorage;
using KristofferStrube.Blazor.FileSystemAccess;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Services;
using ResxManagerExtended.Web.Services;

namespace ResxManagerExtended.Web;

internal static class Bootstrapper
{
    internal static void ConfigureServices(this IServiceCollection collection, bool isDevelopment)
    {
        collection.AddBlazoredLocalStorage();
        collection.AddFileSystemAccessServiceInProcess();

        collection.AddScoped<IResourceService, ResourceService>();
        collection.AddScoped<ISettingService, SettingService>();

        collection.ConfigureNecessaries(isDevelopment);
    }
}