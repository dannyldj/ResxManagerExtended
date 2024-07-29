using Microsoft.JSInterop;
using ResxManagerExtended.Shared.Store.Settings.UseCase;

namespace ResxManagerExtended.Shared.Store.Settings;

public record GetRootResultAction(IJSObjectReference Handler);