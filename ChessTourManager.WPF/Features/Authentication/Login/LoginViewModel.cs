using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.Authentication.Login;

public class LoginViewModel : ViewModelBase
{
    internal static readonly ChessTourContext LoginContext = new();

    private string? _login;

    private LoginCommand? _loginCommand;

    private string _password;

    public LoginViewModel()
    {
        this._login = "petre@live.com";
        this._password = "Qwe123@";

        SuccessLoginEvent.UserSuccessLogin += SuccessLoginEvent_UserSuccessLogin;
    }

    public string? Login
    {
        get { return this._login; }
        set { this.SetField(ref this._login, value); }
    }

    public string Password
    {
        get { return this._password; }
        set { this.SetField(ref this._password, value); }
    }

    public ICommand LoginCommand
    {
        get { return this._loginCommand ??= new LoginCommand(this); }
    }

    public static User? CurrentUser { get; private set; }

    private static void SuccessLoginEvent_UserSuccessLogin(object source, SuccessLoginEventArgs e)
    {
        CurrentUser = e.User;
        SuccessLoginEvent.UserSuccessLogin -= SuccessLoginEvent_UserSuccessLogin;
    }
}
