using System.Windows;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Wpf;

namespace ResxManagerExtended.Desktop;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // https://github.com/dotnet/aspnetcore/issues/52119
    private async void BlazorWebView_Initialized(object? sender, BlazorWebViewInitializedEventArgs e)
    {
        if (sender is not BlazorWebView blazorWebView) return;

        await blazorWebView.WebView.EnsureCoreWebView2Async();
        blazorWebView.WebView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
    }
}