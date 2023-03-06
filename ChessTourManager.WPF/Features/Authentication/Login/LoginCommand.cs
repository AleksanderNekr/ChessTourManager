using System;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Get;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.Authentication.Login;

public class LoginCommand : CommandBase
{
    private readonly LoginViewModel _loginViewModel;

    public LoginCommand(LoginViewModel loginViewModel)
    {
        _loginViewModel = loginViewModel;
    }

    public override void Execute(object? parameter)
    {
        GetResult result = IGetQueries.CreateInstance(LoginViewModel.LoginContext)
                                      .TryGetUserByLoginAndPass(_loginViewModel.Login, _loginViewModel.Password,
                                                                out User? user);
        switch (result)
        {
            case GetResult.Success:
                if (user != null)
                {
                    SuccessLoginEvent.OnUserSuccessLogin(new SuccessLoginEventArgs(user, DateTimeOffset.Now));
                }

                break;
            case GetResult.UserNotFound:
                MessageBox.Show("Неверный логин или пароль!", "Ошибка входа",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
