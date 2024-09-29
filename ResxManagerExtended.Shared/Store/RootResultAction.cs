using Microsoft.FluentUI.AspNetCore.Components;

namespace ResxManagerExtended.Shared.Store;

public record RootResultAction(IEnumerable<ITreeViewItem>? Nodes);