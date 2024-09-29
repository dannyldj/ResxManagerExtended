using Fluxor;

namespace ResxManagerExtended.Shared.Store.UseCase;

public static class Reducers
{
    [ReducerMethod(typeof(GetRootAction))]
    public static ResourceState ReduceGetRootAction(ResourceState state)
    {
        return state with { Nodes = null, IsResourceLoading = true };
    }

    [ReducerMethod]
    public static ResourceState ReduceRootResultAction(ResourceState state, RootResultAction action)
    {
        return state with { Nodes = action.Nodes, IsResourceLoading = false };
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

    [ReducerMethod(typeof(ImportAction))]
    public static ResourceState ReduceImportAction(ResourceState state)
    {
        return state with { IsResourceProcessing = true };
    }

    [ReducerMethod(typeof(ExportAction))]
    public static ResourceState ReduceExportAction(ResourceState state)
    {
        return state with { IsResourceProcessing = true };
    }

    [ReducerMethod(typeof(ProcessDoneAction))]
    public static ResourceState ReduceExportDoneAction(ResourceState state)
    {
        return state with { IsResourceProcessing = false };
    }
}