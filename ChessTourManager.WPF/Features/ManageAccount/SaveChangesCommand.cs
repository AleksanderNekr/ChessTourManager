using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageAccount;

public class SaveChangesCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        ChessTourContext context = new();
        if (LoginViewModel.CurrentUser is null)
        {
            return;
        }

        context.Users.Update(LoginViewModel.CurrentUser);

        context.SaveChanges();

        MessageBox.Show("Изменения сохранены!", "Изменение пользовательских данных",
                        MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
