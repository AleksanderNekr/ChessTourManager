using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Commands;
using ChessTourManager.WPF.Commands.Events;

namespace ChessTourManager.WPF.ViewModels;

public class LoginViewModel : ViewModelBase
{
    public LoginViewModel()
    {
        _login    = string.Empty;
        _password = string.Empty;

        SuccessLoginEvent.UserSuccessLogin += SuccessLoginEvent_UserSuccessLogin;
    }

    private static void SuccessLoginEvent_UserSuccessLogin(SuccessLoginEventArgs e)
    {
        CurrentUser = e.User;
    }

    private string _login;

    public string Login
    {
        get => _login;
        set { SetField(ref _login, value); }
    }

    private string _password;

    public string Password
    {
        get => _password;
        set { SetField(ref _password, value); }
    }

    private LoginCommand? _loginCommand;

    public ICommand LoginCommand => _loginCommand ??= new LoginCommand(this);

    public static User? CurrentUser { get; private set; }
}
