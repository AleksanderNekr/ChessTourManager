using System.ComponentModel;
using System.Windows;
using ChessTourManager.WPF.Features.Authentication.Register;
using ChessTourManager.WPF.Features.ManageTournaments;

namespace ChessTourManager.WPF.Features.Authentication.Login;

/// <inheritdoc cref="System.Windows.Window" />
public partial class AuthWindow
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

    private void AuthWindow_Closing(object? sender, CancelEventArgs e)
    {
        SuccessLoginEvent.UserSuccessLogin -= LoginViewModel_UserSuccessLogin;
    }
}
