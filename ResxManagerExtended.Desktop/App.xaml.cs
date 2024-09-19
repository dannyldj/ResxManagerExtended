using System.Windows;
using Microsoft.Extensions.DependencyInjection;

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
    }
}