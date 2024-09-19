using Fluxor;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store.ResxManager.UseCase;

[FeatureState]
public record ResxManagerState
{
    public IEnumerable<IResourceFile>? Resources { get; init; }
}