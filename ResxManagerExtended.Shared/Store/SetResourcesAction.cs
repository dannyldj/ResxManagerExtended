using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store;

public record SetResourcesAction(IEnumerable<IResourceFile>? Resources);