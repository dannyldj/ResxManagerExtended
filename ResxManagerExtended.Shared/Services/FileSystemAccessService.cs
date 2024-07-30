using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using ResxManagerExtended.Shared.Components;
using ResxManagerExtended.Shared.Interfaces;

namespace ResxManagerExtended.Shared.Services;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class FileSystemAccessService : IFileSystemAccessService
{
    public async Task<JSObject?> GetRootDirectoryHandler()
    {
        return await Routes.GetRootDirectory();
    }

    public string GetHandlerName(JSObject handler)
    {
        return Routes.GetHandlerName(handler);
    }

    public Task<JSObject?> GetResourceHandlers(JSObject handler)
    {
        return Routes.GetResourceFiles(handler);
    }
}