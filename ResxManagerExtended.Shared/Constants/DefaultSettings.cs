using System.Globalization;

namespace ResxManagerExtended.Shared.Constants;

public static class DefaultSettings
{
    public const string DefaultResourceRegex = "^(.+?)\\.(.+?)?\\.?resx$";

    public static CultureInfo DefaultCulture => CultureInfo.InstalledUICulture;

    public static readonly IEnumerable<CultureInfo> AvailableCultures =
        [new CultureInfo("en"), new CultureInfo("ko")];
}