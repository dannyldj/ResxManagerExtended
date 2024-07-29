using Microsoft.JSInterop;
using ResxManagerExtended.Shared.Extensions;
using ResxManagerExtended.Shared.Interfaces;

namespace ResxManagerExtended.Shared.Services;

public class FileSystemAccessService(IJSRuntime jsRuntime) : IFileSystemAccessService
{
    public async Task<IJSObjectReference> GetRootDirectoryHandler()
    {
        var module = await jsRuntime.GetJsModule();
        return await module.InvokeAsync<IJSObjectReference>("getRootDirectory");
    }
}