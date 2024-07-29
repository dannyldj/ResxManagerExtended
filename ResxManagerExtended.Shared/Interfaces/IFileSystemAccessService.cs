using Microsoft.JSInterop;

namespace ResxManagerExtended.Shared.Interfaces;

public interface IFileSystemAccessService
{
    Task<IJSObjectReference> GetRootDirectoryHandler();
}