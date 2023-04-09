using System.Windows;
using ChessTourManager.DataAccess;

namespace ChessTourManager.WPF;

/// <inheritdoc />
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var splashScreen = new SplashScreen("Assets/ChessTourManagerLogo.png");
        splashScreen.Show(true);

        using var context = new ChessTourContext();
        if (!context.Database.CanConnect())
        {
            MessageBox.Show("Нет подключения к интернету. Проверьте подключение к сети и перезапустите приложение.",
                            "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        base.OnStartup(e);
    }
}
