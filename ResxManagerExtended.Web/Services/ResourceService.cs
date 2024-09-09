using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Services;
using ResxManagerExtended.Web.Extensions;

namespace ResxManagerExtended.Web.Services;

internal class ResourceService(IFileSystemAccessServiceInProcess fileSystemAccessService) : IResourceService
{
    public async Task<ITreeViewItem?> SetTopNode()
    {
        try
        {
            var handle = await fileSystemAccessService.ShowDirectoryPickerAsync();
            return await handle.GetResourceFiles();
        }
        catch (Exception)
        {
            // Closing the DirectoryPicker throws an exception.
            return null;
        }
    }
}