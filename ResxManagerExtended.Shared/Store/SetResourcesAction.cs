using System.Collections.Immutable;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store;

public record SetResourcesAction(ImmutableDictionary<string, IResourceFile>? Resources);