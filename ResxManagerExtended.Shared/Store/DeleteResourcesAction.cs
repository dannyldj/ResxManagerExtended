using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store;

public record DeleteResourcesAction(IReadOnlyList<ResourceView> Resources);