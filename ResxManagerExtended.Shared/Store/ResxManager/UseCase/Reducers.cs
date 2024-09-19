using Fluxor;

namespace ResxManagerExtended.Shared.Store.ResxManager.UseCase;

public static class Reducers
{
    [ReducerMethod]
    public static ResxManagerState ReduceSetResourcesAction(ResxManagerState state, SetResourcesAction action)
    {
        return state with { Resources = action.Resources };
    }

    [ReducerMethod]
    public static ResxManagerState ReduceAddResourceAction(ResxManagerState state, AddResourceAction action)
    {
        return state with { Resources = [..state.Resources ?? [], action.Resource] };
    }
}