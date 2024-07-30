using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace ResxManagerExtended.Shared.Components;

[SupportedOSPlatform("browser")]
public partial class Routes
{
    protected override async Task OnInitializedAsync()
    {
        await JSHost.ImportAsync("Module", "/_content/ResxManagerExtended.Shared/script/index.js");
    }

    [JSImport("getRootDirectory", "Module")]
    internal static partial Task<JSObject?> GetRootDirectory();

    [JSImport("getHandlerName", "Module")]
    internal static partial string GetHandlerName(JSObject handler);

    [JSImport("getResourceFiles", "Module")]
    internal static partial Task<JSObject?> GetResourceFiles(JSObject handler);
}