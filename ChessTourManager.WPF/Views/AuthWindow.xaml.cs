using System.Windows;

namespace ChessTourManager.WPF.Views;

/// <summary>
///     Interaction logic for AuthWindow.xaml
/// </summary>
public partial class AuthWindow : Window
{
    public AuthWindow() => InitializeComponent();

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        new RegisterWindow().Show();
        Close();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}
