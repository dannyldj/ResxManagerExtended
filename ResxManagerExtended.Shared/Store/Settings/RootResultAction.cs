using KristofferStrube.Blazor.FileSystem;

namespace ResxManagerExtended.Shared.Store.Settings;

public record RootResultAction(FileSystemDirectoryHandleInProcess Handle, string? Name);