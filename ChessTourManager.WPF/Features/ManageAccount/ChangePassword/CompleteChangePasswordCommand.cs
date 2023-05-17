using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Helpers;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageAccount.ChangePassword;

public class CompleteChangePasswordCommand : CommandBase
{
    private readonly ChangePasswordViewModel _changePasswordViewModel;

    public CompleteChangePasswordCommand(ChangePasswordViewModel changePasswordViewModel)
    {
        this._changePasswordViewModel = changePasswordViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (LoginViewModel.CurrentUser is null)
        {
            MessageBox.Show("Вы некорректно авторизованы. Пожалуйста, перезайдите в систему.",
                            "Ошибка авторизации",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!PasswordHasher.VerifyPassword(this._changePasswordViewModel.CurrentPassword,
                                           LoginViewModel.CurrentUser.PasswordHash))
        {
            MessageBox.Show("Вы ввели неверный текущий пароль. Пожалуйста, попробуйте ещё раз.",
                            "Ошибка ввода пароля",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (this._changePasswordViewModel.NewPassword != this._changePasswordViewModel.RepeatPassword)
        {
            MessageBox.Show("Вы ввели разные пароли. Пожалуйста, попробуйте ещё раз.",
                            "Ошибка ввода пароля",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        bool isNewEmpty    = this._changePasswordViewModel.NewPassword.Length == 0;
        bool isRepeatEmpty = this._changePasswordViewModel.RepeatPassword.Length == 0;
        if (isNewEmpty || isRepeatEmpty)
        {
            MessageBox.Show("Вы не ввели новый пароль. Пожалуйста, попробуйте ещё раз.",
                            "Ошибка ввода пароля",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        ChessTourContext context = new();
        LoginViewModel.CurrentUser.PasswordHash = PasswordHasher.HashPassword(this._changePasswordViewModel.NewPassword);
        context.Users.Update(LoginViewModel.CurrentUser);
        context.SaveChanges();

        MessageBox.Show("Пароль успешно изменён.", "Изменение пароля",
                        MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
