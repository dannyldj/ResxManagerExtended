using System.Globalization;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Store;

public record EditResourceAction(ResourceView Resource, CultureInfo Culture, string Value);