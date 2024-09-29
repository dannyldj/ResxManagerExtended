using Fluxor;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store.UseCase;

[FeatureState]
public record ResourceState
{
    public IEnumerable<ITreeViewItem>? Nodes { get; init; }

    public IReadOnlyDictionary<string, IResourceFile>? Resources { get; init; }

    public bool IsResourceLoading { get; init; }

    public string? Regex { get; init; }

    public bool IsResourceProcessing { get; init; }
}