using System.Runtime.InteropServices.JavaScript;

namespace ResxManagerExtended.Shared.Store.Settings;

public record GetRootResultAction(JSObject? Handler, string? Name);