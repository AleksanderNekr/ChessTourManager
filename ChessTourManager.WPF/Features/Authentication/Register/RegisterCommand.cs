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
        this._registerViewModel = registerViewModel;
    }

    /// <inheritdoc />
    public override void Execute(object? parameter)
    {
        if (this._registerViewModel.PasswordInit != this._registerViewModel.PasswordConfirm)
        {
            MessageBox.Show("Пароли не совпадают!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (this._registerViewModel is { Email: null } or { PasswordInit: null })
        {
            MessageBox.Show("Не удалось зарегистрироваться! Проблема с логином или паролем",
                            "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (this._registerViewModel is { LastName: null } or { FirstName: null })
        {
            MessageBox.Show("Имя и фамилия должны быть заполнены!",
                            "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        InsertResult result = IInsertQueries.CreateInstance(RegisterViewModel.RegisterContext)
                                            .TryAddUser(out User? user, this._registerViewModel.LastName,
                                                        this._registerViewModel.FirstName,
                                                        this._registerViewModel.Email,
                                                        this._registerViewModel.PasswordInit,
                                                        this._registerViewModel.Patronymic ?? string.Empty);


        if (result == InsertResult.Success)
        {
            MessageBox.Show("Вы успешно зарегистрировались!",
                            "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
            SuccessRegisterEvent.OnUserSuccessRegister(this,
                                                       new SuccessRegisterEventArgs(user, DateTime.UtcNow));
            return;
        }

        MessageBox.Show("Не удалось зарегистрироваться! Возможно пользователь с такими данными уже существует",
                        "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
