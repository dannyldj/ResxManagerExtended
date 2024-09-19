using Fluxor;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

public static class Reducers
{
    [ReducerMethod(typeof(GetRootAction))]
    public static SettingState ReduceGetRootAction(SettingState state)
    {
        return state with { TopNode = null, IsResourceLoading = true };
    }

    [ReducerMethod]
    public static SettingState ReduceRootResultAction(SettingState state, RootResultAction action)
    {
        return state with { TopNode = action.TopNode, IsResourceLoading = false };
    }

    [ReducerMethod]
    public static SettingState ReduceRegexResultAction(SettingState state, RegexResultAction action)
    {
        return state with { Regex = action.Regex };
    }
}