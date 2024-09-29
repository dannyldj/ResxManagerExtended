using System.Globalization;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store;

public record ExportAction(IReadOnlyList<CultureInfo> Cultures, IEnumerable<ResourceView> Resources);