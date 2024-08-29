using Fluxor;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

public static class Reducers
{
    [ReducerMethod(typeof(GetRootAction))]
    public static SettingState ReduceGetRootAction(SettingState state)
    {
        return state with { DirectoryHandle = null, DirectoryName = null };
    }

    [ReducerMethod]
    public static SettingState ReduceRootResultAction(SettingState state, RootResultAction action)
    {
        return state with { DirectoryHandle = action.Handle, DirectoryName = action.Name };
    }

    [ReducerMethod]
    public static SettingState ReduceRegexResultAction(SettingState state, RegexResultAction action)
    {
        return state with { Regex = action.Regex };
    }
}