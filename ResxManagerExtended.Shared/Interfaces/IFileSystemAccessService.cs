using System.Runtime.InteropServices.JavaScript;

namespace ResxManagerExtended.Shared.Interfaces;

public interface IFileSystemAccessService
{
    Task<JSObject?> GetRootDirectoryHandler();

    string GetHandlerName(JSObject handler);

    Task<JSObject?> GetResourceHandlers(JSObject handler);
}