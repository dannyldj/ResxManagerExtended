using Fluxor;
using Microsoft.FluentUI.AspNetCore.Components;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

[FeatureState]
public record SettingState
{
    public ITreeViewItem? TopNode { get; init; }

    public bool IsResourceLoading { get; init; }

    public string? Regex { get; init; }
}