using System.Globalization;
using System.Resources;
using ResxManagerExtended.Shared.Properties;

namespace ResxManagerExtended.Shared.Constants;

public static class DefaultSettings
{
    public const string ResourceResxName = "name", ResourceResxCode = "code";
    public const string DefaultResxRegex = $@"^(?<{ResourceResxName}>.+?)\.(?<{ResourceResxCode}>.+?)?\.?resx$";

    public static CultureInfo DefaultCulture => CultureInfo.InstalledUICulture;

    public static IEnumerable<CultureInfo> AvailableCultures
    {
        get
        {
            var result = new List<CultureInfo>();
            var rm = new ResourceManager(typeof(Resources));
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (var culture in cultures)
            {
                try
                {
                    if (culture.Equals(CultureInfo.InvariantCulture)) continue;

                    var rs = rm.GetResourceSet(culture, true, false);
                    if (rs != null)
                        result.Add(culture);
                }
                catch (CultureNotFoundException)
                {
                    // Exceptions occur in some cultures.
                }
            }

            return [new CultureInfo("en"), ..result];
        }
    }
}