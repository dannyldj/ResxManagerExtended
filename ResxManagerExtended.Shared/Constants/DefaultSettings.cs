using System.Globalization;

namespace ResxManagerExtended.Shared.Constants;

public static class DefaultSettings
{
    public const string DefaultResourceRegex = "^(.+?)\\.(.+?)?\\.?resx$";

    public static readonly IEnumerable<CultureInfo> AvailableCultures = [new("en"), new("ko")];

    public static CultureInfo DefaultCulture => CultureInfo.InstalledUICulture;
}