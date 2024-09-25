using Fluxor;

namespace ResxManagerExtended.Shared.Store.UseCase;

public static class Reducers
{
    [ReducerMethod(typeof(GetRootAction))]
    public static ResourceState ReduceGetRootAction(ResourceState state)
    {
        return state with { TopNode = null, IsResourceLoading = true };
    }

    [ReducerMethod]
    public static ResourceState ReduceRootResultAction(ResourceState state, RootResultAction action)
    {
        return state with { TopNode = action.TopNode, IsResourceLoading = false };
    }

    [ReducerMethod]
    public static ResourceState ReduceRegexResultAction(ResourceState state, RegexResultAction action)
    {
        return state with { Regex = action.Regex };
    }

    [ReducerMethod]
    public static ResourceState ReduceSetResourcesAction(ResourceState state, SetResourcesAction action)
    {
        return state with { Resources = action.Resources };
    }

    [ReducerMethod(typeof(ExportAction))]
    public static ResourceState ReduceExportAction(ResourceState state)
    {
        return state with { IsResourceExporting = true };
    }

    [ReducerMethod(typeof(ExportDoneAction))]
    public static ResourceState ReduceExportDoneAction(ResourceState state)
    {
        return state with { IsResourceExporting = false };
    }
}