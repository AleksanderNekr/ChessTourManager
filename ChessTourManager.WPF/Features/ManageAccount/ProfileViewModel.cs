using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageAccount;

public class ProfileViewModel : ViewModelBase
{
    public string GreetMessage
    {
        get { return $"Добро пожаловать, {FirstName}!"; }
    }

    public string? FirstName
    {
        get { return LoginViewModel.CurrentUser?.UserFirstName; }
        set
        {
            if (LoginViewModel.CurrentUser != null)
            {
                LoginViewModel.CurrentUser.UserFirstName = value!;
            }
        }
    }

    public string? LastName
    {
        get { return LoginViewModel.CurrentUser?.UserLastName; }
        set
        {
            if (LoginViewModel.CurrentUser != null)
            {
                LoginViewModel.CurrentUser.UserLastName = value!;
            }
        }
    }

    public string? Email
    {
        get { return LoginViewModel.CurrentUser?.Email; }
        set
        {
            if (LoginViewModel.CurrentUser != null)
            {
                LoginViewModel.CurrentUser.Email = value!;
            }
        }
    }

    public string? Patronymic
    {
        get { return LoginViewModel.CurrentUser?.UserPatronymic; }
        set
        {
            if (LoginViewModel.CurrentUser != null)
            {
                LoginViewModel.CurrentUser.UserPatronymic = value!;
            }
        }
    }
}
