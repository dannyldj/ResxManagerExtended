using System.Globalization;
using System.Xml.Linq;

namespace ResxManagerExtended.Shared.Extensions;

public static class XDocumentExtension
{
    public static Dictionary<string, string?> GetResources(this XDocument document, CultureInfo culture)
    {
        return document.Descendants("data")
            .Where(e => e.Attribute("name") is not null)
            .Select(e => new { Key = e.Attribute("name")!.Value, e.Element("value")?.Value })
            .ToDictionary(e => e.Key, e => e.Value);
    }
}