using System.Windows;

namespace ChessTourManager.WPF;

/// <inheritdoc />
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var splashScreen = new SplashScreen("Assets/ChessTourManagerLogo.png");
        splashScreen.Show(true);

        base.OnStartup(e);
    }
}
