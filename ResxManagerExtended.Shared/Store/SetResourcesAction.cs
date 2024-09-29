using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store;

public record SetResourcesAction(IReadOnlyDictionary<string, IResourceFile>? Resources);