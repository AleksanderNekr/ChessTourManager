using System;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.Authentication.Register;

public class RegisterCommand : CommandBase
{
    private readonly RegisterViewModel _registerViewModel;

    public RegisterCommand(RegisterViewModel registerViewModel)
    {
        _registerViewModel = registerViewModel;
    }

    /// <inheritdoc />
    public override void Execute(object? parameter)
    {
        if (_registerViewModel.PasswordInit != _registerViewModel.PasswordConfirm)
        {
            MessageBox.Show("Пароли не совпадают!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (_registerViewModel is { Email: null } || _registerViewModel is { PasswordInit: null })
        {
            MessageBox.Show("Не удалось зарегистрироваться! Проблема с логином или паролем",
                            "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (_registerViewModel is { LastName: null } || _registerViewModel is { FirstName: null })
        {
            MessageBox.Show("Имя и фамилия должны быть заполнены!",
                            "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        InsertResult result = IInsertQueries.CreateInstance(RegisterViewModel.RegisterContext)
                                            .TryAddUser(out User? user, _registerViewModel.LastName,
                                                        _registerViewModel.FirstName,
                                                        _registerViewModel.Email,
                                                        _registerViewModel.PasswordInit,
                                                        _registerViewModel.Patronymic ?? string.Empty);

        switch (result)
        {
            case InsertResult.Success:
                SuccessRegisterEvent
                   .OnUserSuccessRegister(new SuccessRegisterEventArgs(user, DateTimeOffset.UtcNow));
                MessageBox.Show("Вы успешно зарегистрировались!",
                                "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                break;
            case InsertResult.Fail:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
