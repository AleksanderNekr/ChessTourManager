using System;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.Authentication.Login;

public class LoginCommand : CommandBase
{
    private readonly LoginViewModel _loginViewModel;

    public LoginCommand(LoginViewModel loginViewModel)
    {
        this._loginViewModel = loginViewModel;
    }

    public override void Execute(object? parameter)
    {
        GetResult result = IGetQueries.CreateInstance(LoginViewModel.LoginContext)
                                      .TryGetUserByLoginAndPass(this._loginViewModel.Login, this._loginViewModel.Password,
                                                                out User? user);
        if (result == GetResult.Success)
        {
            SuccessLoginEvent.OnUserSuccessLogin(this, new SuccessLoginEventArgs(user, DateTimeOffset.Now));
            return;
        }

        MessageBox.Show("Неверный логин или пароль!", "Ошибка входа",
                        MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
