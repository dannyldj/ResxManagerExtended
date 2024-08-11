using KristofferStrube.Blazor.FileSystem;

namespace ResxManagerExtended.Shared.Store.Settings;

public record GetRootResultAction(FileSystemDirectoryHandleInProcess Handle, string? Name);