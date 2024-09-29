using System.Globalization;
using CsvHelper;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Extensions;

public static class CsvExtension
{
    public static async Task ExportCsvAsync(this CsvWriter csv, IReadOnlyList<CultureInfo> cultures,
        IEnumerable<ResourceView> resources)
    {
        csv.WriteHeader(typeof(ResourceView));
        foreach (var culture in cultures)
        {
            csv.WriteField(culture.Name);
        }

        await csv.NextRecordAsync();

        foreach (var resource in resources)
        {
            csv.WriteRecord(resource);

            foreach (var culture in cultures)
            {
                csv.WriteField(resource.Columns.GetValueOrDefault(culture));
            }

            await csv.NextRecordAsync();
        }

        await csv.FlushAsync();
    }
}