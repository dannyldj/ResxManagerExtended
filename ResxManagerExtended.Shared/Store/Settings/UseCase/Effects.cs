using Blazored.LocalStorage;
using Fluxor;
using KristofferStrube.Blazor.FileSystemAccess;
using ResxManagerExtended.Shared.Constants;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

public class Effects(
    IFileSystemAccessServiceInProcess fileSystemAccessService,
    ILocalStorageService localStorageService)
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

    [EffectMethod(typeof(GetRegexAction))]
    public async Task HandleGetRegexAction(IDispatcher dispatcher)
    {
        var regex = await localStorageService.GetItemAsStringAsync(LocalStorageKeys.ResourceRegexKey);
        dispatcher.Dispatch(new GetRegexResultAction(regex ?? DefaultSettings.DefaultResourceRegex));
    }

    [EffectMethod]
    public async Task HandleSetRegexAction(SetRegexAction action, IDispatcher dispatcher)
    {
        if (action.Regex is null) return;

        await localStorageService.SetItemAsStringAsync(LocalStorageKeys.ResourceRegexKey, action.Regex);
        dispatcher.Dispatch(new GetRegexResultAction(action.Regex));
    }
}