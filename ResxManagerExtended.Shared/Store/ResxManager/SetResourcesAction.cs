using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store.ResxManager;

public record SetResourcesAction(IEnumerable<IResourceFile>? Resources);