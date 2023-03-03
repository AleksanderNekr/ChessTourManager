using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.Authentication.Login;

public class LoginViewModel : ViewModelBase
{
    internal static readonly ChessTourContext LoginContext = new();

    private string _login;

    private LoginCommand? _loginCommand;

    private string _password;

    public LoginViewModel()
    {
        _login    = "petre@live.com";
        _password = "123qwe";

        SuccessLoginEvent.UserSuccessLogin += SuccessLoginEvent_UserSuccessLogin;
    }

    public string Login
    {
        get => _login;
        set => SetField(ref _login, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public ICommand LoginCommand => _loginCommand ??= new LoginCommand(this);

    public static User? CurrentUser { get; private set; }

    private static void SuccessLoginEvent_UserSuccessLogin(SuccessLoginEventArgs e) => CurrentUser = e.User;
}
