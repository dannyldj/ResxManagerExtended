using KristofferStrube.Blazor.FileSystem;

namespace ResxManagerExtended.Shared.Data;

public record Resource(
    string Path,
    FileSystemDirectoryHandleInProcess Handle,
    IEnumerable<FileSystemFileHandleInProcess> Resources);