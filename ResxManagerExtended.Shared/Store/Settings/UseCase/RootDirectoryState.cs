using Microsoft.JSInterop;

namespace ResxManagerExtended.Shared.Store.Settings.UseCase;

public record RootDirectoryState
{
    public IJSObjectReference? DirectoryHandler { get; init; }
}