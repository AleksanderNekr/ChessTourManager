using System.ComponentModel;
using System.Windows;
using ChessTourManager.WPF.Features.Authentication.Login;

namespace ChessTourManager.WPF.Features.Authentication.Register;

/// <inheritdoc cref="System.Windows.Window" />
public partial class RegisterWindow
{
    public RegisterWindow()
    {
        this.InitializeComponent();
        SuccessRegisterEvent.UserSuccessRegister += this.RegisterViewModel_UserSuccessRegister;
    }

    private void RegisterViewModel_UserSuccessRegister(object source, SuccessRegisterEventArgs successRegisterEventArgs)
    {
        new AuthWindow().Show();
        this.Close();
    }

    private void ReturnToAuthWindow_Click(object sender, RoutedEventArgs e)
    {
        new AuthWindow().Show();
        this.Close();
    }

    private void RegisterWindow_Closing(object? sender, CancelEventArgs e)
    {
        SuccessRegisterEvent.UserSuccessRegister -= this.RegisterViewModel_UserSuccessRegister;
    }
}
