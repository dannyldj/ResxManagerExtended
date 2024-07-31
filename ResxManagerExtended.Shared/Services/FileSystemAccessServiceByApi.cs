using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using ResxManagerExtended.Shared.Components;
using ResxManagerExtended.Shared.Interfaces;

namespace ResxManagerExtended.Shared.Services;

[SupportedOSPlatform("browser")]
public class FileSystemAccessServiceByApi : IFileSystemAccessService
{
    public async Task<JSObject?> GetRootDirectoryHandler()
    {
        return await JsInteropInitializer.GetRootDirectory();
    }

    public string GetHandlerName(JSObject handler)
    {
        return JsInteropInitializer.GetHandlerName(handler);
    }

    public Task<JSObject?> GetResourceHandlers(JSObject handler)
    {
        return JsInteropInitializer.GetResourceFiles(handler);
    }
}