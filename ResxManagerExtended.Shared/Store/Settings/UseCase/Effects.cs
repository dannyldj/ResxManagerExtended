using Fluxor;
using KristofferStrube.Blazor.FileSystemAccess;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

public class Effects(IFileSystemAccessServiceInProcess fileSystemAccessService)
{
    [EffectMethod(typeof(GetRootAction))]
    public async Task HandleGetRootAction(IDispatcher dispatcher)
    {
        try
        {
            var handle = await fileSystemAccessService.ShowDirectoryPickerAsync();
            var directoryName = await handle.GetNameAsync();
            dispatcher.Dispatch(new GetRootResultAction(handle, directoryName));
        }
        catch (Exception)
        {
            // Closing the DirectoryPicker throws an exception.
        }
    }
}