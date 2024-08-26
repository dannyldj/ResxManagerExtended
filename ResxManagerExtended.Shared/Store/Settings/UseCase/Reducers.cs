using Fluxor;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

public static class Reducers
{
    [ReducerMethod(typeof(GetRootAction))]
    public static RootDirectoryState ReduceGetRootAction(RootDirectoryState state)
    {
        return new RootDirectoryState();
    }

    [ReducerMethod]
    public static RootDirectoryState ReduceGetRootResultAction(RootDirectoryState state, GetRootResultAction action)
    {
        return new RootDirectoryState { DirectoryHandle = action.Handle, DirectoryName = action.Name };
    }

    [ReducerMethod]
    public static ResourceRegexState ReduceGetRegexResultAction(ResourceRegexState state, GetRegexResultAction action)
    {
        return new ResourceRegexState { Regex = action.Regex };
    }
}