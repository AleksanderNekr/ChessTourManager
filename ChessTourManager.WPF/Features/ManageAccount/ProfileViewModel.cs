using System.Windows.Input;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageAccount.ChangePassword;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageAccount;

public class ProfileViewModel : ViewModelBase
{
    private ChangePasswordCommand? _changePasswordCommand;
    private SaveChangesCommand?    _saveChanges;

    public string GreetMessage
    {
        get { return $"Добро пожаловать, {this.FirstName}"; }
    }

    public string? FirstName
    {
        get { return LoginViewModel.CurrentUser?.UserFirstName; }
        set
        {
            if (LoginViewModel.CurrentUser is not null)
            {
                LoginViewModel.CurrentUser.UserFirstName = value;
                this.OnPropertyChanged(nameof(this.GreetMessage));
            }
        }
    }

    public string? LastName
    {
        get { return LoginViewModel.CurrentUser?.UserLastName; }
        set
        {
            if (LoginViewModel.CurrentUser is not null)
            {
                LoginViewModel.CurrentUser.UserLastName = value;
            }
        }
    }

    public string? Email
    {
        get { return LoginViewModel.CurrentUser?.Email; }
        set
        {
            if (LoginViewModel.CurrentUser is not null)
            {
                LoginViewModel.CurrentUser.Email = value;
            }
        }
    }

    public string? Patronymic
    {
        get { return LoginViewModel.CurrentUser?.UserPatronymic; }
        set
        {
            if (LoginViewModel.CurrentUser is not null)
            {
                LoginViewModel.CurrentUser.UserPatronymic = value;
            }
        }
    }

    public string RegisterDateText
    {
        get
        {
            return @$"Дата регистрации: {
                LoginViewModel.CurrentUser?.RegisterDate.ToString("dd.MM.yyyy")
             ?? string.Empty}";
        }
    }

    public ICommand SaveChanges
    {
        get { return this._saveChanges ??= new SaveChangesCommand(); }
    }

    public ICommand ChangePasswordCommand
    {
        get { return this._changePasswordCommand ??= new ChangePasswordCommand(); }
    }
}
