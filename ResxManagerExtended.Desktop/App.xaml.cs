using System.Globalization;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ResxManagerExtended.Desktop.Properties;
using ResxManagerExtended.Shared.Constants;

namespace ResxManagerExtended.Desktop;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();
#if DEBUG
        serviceCollection.ConfigureServices(true);
#else
        serviceCollection.ConfigureServices(false);
#endif
        Resources.Add("services", serviceCollection.BuildServiceProvider());

        SetCulture();
    }

    private static void SetCulture()
    {
        var culture = string.IsNullOrEmpty(Settings.Default.BlazorCulture)
            ? DefaultSettings.DefaultCulture
            : CultureInfo.GetCultureInfo(Settings.Default.BlazorCulture);

        if (string.IsNullOrEmpty(Settings.Default.BlazorCulture))
            Settings.Default.BlazorCulture = DefaultSettings.DefaultCulture.Name;

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}