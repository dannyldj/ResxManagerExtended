using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ResxManagerExtended.Shared.Data;

public class ResourceView
{
    public required string Path { get; init; }

    public required string Key { get; init; }

    public string? Comment { get; set; }

    public required Dictionary<CultureInfo, string?> Columns { get; init; }
}

public sealed class ResourceViewMap : ClassMap<ResourceView>
{
    public ResourceViewMap()
    {
        Map(m => m.Path);
        Map(m => m.Key);
        Map(m => m.Comment);
        Map(m => m.Columns).TypeConverter<ResourceDictionaryConverter>().Name(string.Empty);
    }
}

public class ResourceDictionaryConverter : IDictionaryConverter
{
    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        var dictionary = new Dictionary<CultureInfo, string?>();
        var indexEnd = memberMapData.IndexEnd < memberMapData.Index ? row.Parser.Count - 1 : memberMapData.IndexEnd;

        for (var i = memberMapData.Index; i <= indexEnd; i++)
        {
            if (row.TryGetField(i, out string? field)) dictionary.Add(new CultureInfo(row.HeaderRecord![i]), field);
        }

        return dictionary;
    }
}