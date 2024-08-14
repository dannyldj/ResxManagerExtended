using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace ResxManagerExtended.Shared.Extensions;

public static class Bootstrapper
{
    public static void ConfigureServices(this IServiceCollection collection)
    {
        collection.AddLocalization();
        collection.AddFluentUIComponents();

        collection.AddFileSystemAccessServiceInProcess();
    }
}