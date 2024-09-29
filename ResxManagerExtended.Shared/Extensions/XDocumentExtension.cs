using System.Xml.Linq;

namespace ResxManagerExtended.Shared.Extensions;

public static class XDocumentExtension
{
    public static IEnumerable<Resource> GetResources(this XDocument document)
    {
        return document.Descendants("data")
            .Where(e => e.Attribute("name") is not null)
            .Select(e =>
                new Resource(e.Attribute("name")!.Value, e.Element("comment")?.Value, e.Element("value")?.Value));
    }

    public static XElement? GetNode(this XDocument document, string name)
    {
        return document.Descendants("data").Where(e => string.Equals(e.Attribute("name")?.Value, name))
            .Select(e => e.Element("value")).SingleOrDefault();
    }

    public record Resource(string Key, string? Comment, string? Value);
}