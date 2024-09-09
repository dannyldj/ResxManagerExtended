using KristofferStrube.Blazor.FileSystem;
using Microsoft.FluentUI.AspNetCore.Components;

namespace ResxManagerExtended.Web.Data;

public class ResourceTreeViewItem : ITreeViewItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResourceTreeViewItem" /> class.
    /// </summary>
    public ResourceTreeViewItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ResourceTreeViewItem" /> class.
    /// </summary>
    /// <param name="text">Text of the tree item</param>
    /// <param name="handle">Directory handle of the tree item</param>
    /// <param name="items">Sub-items of the tree item.</param>
    public ResourceTreeViewItem(string text, IFileSystemHandleInProcess? handle = null,
        IEnumerable<ITreeViewItem>? items = null)
    {
        Text = text;
        Handle = handle;
        Items = items;
    }

    /// <summary>
    ///     Returns a <see cref="ResourceTreeViewItem" /> that represents a loading state.
    /// </summary>
    public static ResourceTreeViewItem LoadingTreeViewItem =>
        new() { Text = FluentTreeView.LoadingMessage, Disabled = true };

    /// <summary>
    ///     Returns an array with a single <see cref="ResourceTreeViewItem" /> that represents a loading state.
    /// </summary>
    public static IEnumerable<ResourceTreeViewItem> LoadingTreeViewItems =>
    [
        new() { Text = FluentTreeView.LoadingMessage, Disabled = true }
    ];

    /// <summary>
    ///     Gets or sets the directory handle of the tree item.
    /// </summary>
    public IFileSystemHandleInProcess? Handle { get; set; }

    /// <summary>
    ///     Gets or sets the selectability of the tree item.
    /// </summary>
    public bool IsSelectable { get; set; }

    /// <summary>
    ///     <inheritdoc cref="ITreeViewItem.Id" />
    /// </summary>
    public string Id { get; set; } = Identifier.NewId();

    /// <summary>
    ///     <inheritdoc cref="ITreeViewItem.Text" />
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     <inheritdoc cref="ITreeViewItem.Items" />
    /// </summary>
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    ///     <inheritdoc cref="ITreeViewItem.IconCollapsed" />
    /// </summary>
    public Icon? IconCollapsed { get; set; }

    /// <summary>
    ///     <inheritdoc cref="ITreeViewItem.IconExpanded" />
    /// </summary>
    public Icon? IconExpanded { get; set; }

    /// <summary>
    ///     <inheritdoc cref="ITreeViewItem.Disabled" />
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    ///     <inheritdoc cref="ITreeViewItem.Expanded" />
    /// </summary>
    public bool Expanded { get; set; } = false;

    /// <summary>
    ///     <inheritdoc cref="ITreeViewItem.OnExpandedAsync" />
    /// </summary>
    public Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get; set; }
}