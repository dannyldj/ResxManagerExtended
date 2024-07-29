using Microsoft.JSInterop;

namespace ResxManagerExtended.Shared.Extensions;

public static class JsModuleExtension
{
    public static async Task<IJSObjectReference> GetJsModule(this IJSRuntime runtime)
    {
        return await runtime.InvokeAsync<IJSObjectReference>("import",
            "/_content/ResxManagerExtended.Shared/script/module.js");
    }
}