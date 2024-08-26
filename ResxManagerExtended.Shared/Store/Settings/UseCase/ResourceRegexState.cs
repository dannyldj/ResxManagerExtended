using Fluxor;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

[FeatureState]
public class ResourceRegexState
{
    public string? Regex { get; init; }
}