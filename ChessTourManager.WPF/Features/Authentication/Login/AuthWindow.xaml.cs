using System.Windows;
using ChessTourManager.WPF.Features.Authentication.Register;
using ChessTourManager.WPF.Features.ManageTournaments;

namespace ChessTourManager.WPF.Features.Authentication.Login;

/// <summary>
///     Interaction logic for AuthWindow.xaml
/// </summary>
public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        SuccessLoginEvent.UserSuccessLogin += LoginViewModel_UserSuccessLogin;
    }

    private void LoginViewModel_UserSuccessLogin(SuccessLoginEventArgs successLoginEventArgs)
    {
        new MainWindow().Show();
        Close();
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        new RegisterWindow().Show();
        Close();
    }
}
