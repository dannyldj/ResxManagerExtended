using Fluxor;
using ResxManagerExtended.Shared.Interfaces;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

public class Effects(IFileSystemAccessService fileSystemAccessService)
{
    [EffectMethod(typeof(GetRootAction))]
    public async Task HandleGetRootAction(IDispatcher dispatcher)
    {
        var handler = await fileSystemAccessService.GetRootDirectoryHandler();
        dispatcher.Dispatch(new GetRootResultAction(handler));
    }
}