using System.Globalization;

namespace ResxManagerExtended.Shared.Data;

public interface IResourceFile
{
    string Path { get; }

    string Name { get; }

    IEnumerable<CultureInfo>? Cultures { get; }

    public string GetResourcePath();

    Task SetValue(string key, CultureInfo culture, string value, CancellationToken token = default);

    Task SetValue(string key, IDictionary<CultureInfo, string?> cultures, CancellationToken token = default);

    Task<IEnumerable<ResourceView>> GetValues(CancellationToken token = default);

    public static bool DetectUtf8Bom(byte[] byteArray)
    {
        if (byteArray.Length < 3) return false;

        return byteArray[0] == 0xEF && byteArray[1] == 0xBB && byteArray[2] == 0xBF;
    }

    public static bool DetectUtf8Bom(string filePath)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var bom = new byte[3];
        _ = fs.Read(bom, 0, 3);

        return DetectUtf8Bom(bom);
    }
}