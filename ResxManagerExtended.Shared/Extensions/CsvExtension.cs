using System.Collections.Immutable;
using System.Globalization;
using CsvHelper;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Extensions;

public static class CsvExtension
{
    public static void ExportCsv(this CsvWriter csv, ImmutableArray<CultureInfo> cultures,
        IEnumerable<ResourceView> resources)
    {
        csv.WriteHeader(typeof(ResourceView));
        foreach (var culture in cultures)
        {
            csv.WriteField(culture.Name);
        }

        csv.NextRecord();

        foreach (var resource in resources)
        {
            csv.WriteRecord(resource);

            foreach (var culture in cultures)
            {
                csv.WriteField(resource.Columns.GetValueOrDefault(culture));
            }

            csv.NextRecord();
        }
    }
}