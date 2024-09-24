using System.Collections.Immutable;
using System.Globalization;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store;

public record ExportAction(ImmutableArray<CultureInfo> Cultures, IEnumerable<ResourceView> Resources);