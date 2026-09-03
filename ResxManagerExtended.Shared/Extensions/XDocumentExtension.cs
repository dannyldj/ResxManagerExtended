using System.Xml.Linq;

namespace ResxManagerExtended.Shared.Extensions;

public static class XDocumentExtension
{
    private const string IndentUnit = "  ";

    extension(XDocument document)
    {
        public IEnumerable<Resource> GetResources()
        {
            return document.Descendants("data")
                .Where(e => e.Attribute("name") is not null)
                .Select(e =>
                    new Resource(e.Attribute("name")!.Value, e.Element("comment")?.Value, e.Element("value")?.Value));
        }

        public XElement? GetDataNode(string name)
        {
            return document.Descendants("data")
                .SingleOrDefault(e => string.Equals(e.Attribute("name")?.Value, name));
        }

        public void SetResourceValue(string name, string value)
        {
            var data = document.GetDataNode(name);
            if (data is null)
            {
                document.AddDataNode(name, value);
                return;
            }

            var node = data.Element("value");
            if (node is null)
            {
                data.Add(new XElement("value", value));
            }
            else
            {
                node.SetValue(value);
            }
        }

        public bool RemoveResource(string name)
        {
            var data = document.GetDataNode(name);
            if (data is null)
            {
                return false;
            }

            // 노드 앞 들여쓰기 공백까지 지워야 빈 줄이 남지 않는다.
            if (data.PreviousNode is XText indent)
            {
                indent.Remove();
            }

            data.Remove();

            return true;
        }

        private void AddDataNode(string name, string value)
        {
            var data = new XElement("data",
                new XAttribute("name", name),
                new XAttribute(XNamespace.Xml + "space", "preserve"));

            var last = document.Descendants("data").LastOrDefault();
            var indent = (last?.PreviousNode as XText)?.Value;

            if (indent is null)
            {
                data.Add(new XElement("value", value));
            }
            else
            {
                data.Add(new XText(indent + IndentUnit), new XElement("value", value), new XText(indent));
            }

            if (last is null)
            {
                document.Root?.Add(data);
            }
            else if (indent is null)
            {
                last.AddAfterSelf(data);
            }
            else
            {
                last.AddAfterSelf(new XText(indent), data);
            }
        }
    }

    public record Resource(string Key, string? Comment, string? Value);
}