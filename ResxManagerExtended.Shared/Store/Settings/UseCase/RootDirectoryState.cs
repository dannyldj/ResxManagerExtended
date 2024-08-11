using Fluxor;
using KristofferStrube.Blazor.FileSystem;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

[FeatureState]
public record RootDirectoryState
{
    public FileSystemDirectoryHandleInProcess? DirectoryHandle { get; init; }

    public string? DirectoryName { get; init; }
}