using System.Runtime.InteropServices.JavaScript;
using Fluxor;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

[FeatureState]
public record RootDirectoryState
{
    public JSObject? DirectoryHandler { get; init; }
    
    public string? DirectoryName { get; init; }
}