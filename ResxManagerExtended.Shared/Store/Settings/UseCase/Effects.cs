using Fluxor;
using KristofferStrube.Blazor.FileSystemAccess;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

public class Effects(IFileSystemAccessServiceInProcess fileSystemAccessService)
{
    [EffectMethod(typeof(GetRootAction))]
    public async Task HandleGetRootAction(IDispatcher dispatcher)
    {
        var handle = await fileSystemAccessService.ShowDirectoryPickerAsync();
        var directoryName = await handle.GetNameAsync();
        dispatcher.Dispatch(new GetRootResultAction(handle, directoryName));
    }
}