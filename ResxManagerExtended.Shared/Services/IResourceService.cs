using Microsoft.FluentUI.AspNetCore.Components;

namespace ResxManagerExtended.Shared.Services;

public interface IResourceService
{
    Task<ITreeViewItem?> SetTopNode();
}