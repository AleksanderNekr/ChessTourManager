using System.ComponentModel;
using System.Windows;
using ChessTourManager.WPF.Features.Authentication.Login;

namespace ChessTourManager.WPF.Features.Authentication.Register;

/// <summary>
///     Interaction logic for RegisterWindow.xaml
/// </summary>
public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
        SuccessRegisterEvent.UserSuccessRegister += RegisterViewModel_UserSuccessRegister;
    }

    private void RegisterViewModel_UserSuccessRegister(SuccessRegisterEventArgs e)
    {
        new AuthWindow().Show();
        Close();
    }

    private void ReturnToAuthWindow_Click(object sender, RoutedEventArgs e)
    {
        new AuthWindow().Show();
        Close();
    }

    private void RegisterWindow_Closing(object? sender, CancelEventArgs e)
    {
        SuccessRegisterEvent.UserSuccessRegister -= RegisterViewModel_UserSuccessRegister;
    }
}
