using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Delete;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;

public class DeletePlayerCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is not Player player)
        {
            return;
        }

        MessageBoxResult isSure = MessageBox.Show("Вы уверены, что хотите удалить игрока "
                                                + $"{player.PlayerFullName}?", "Удаление игрока",
                                                  MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (isSure == MessageBoxResult.No)
        {
            return;
        }

        DeleteResult result = IDeleteQueries.CreateInstance(PlayersViewModel.PlayersContext)
                                            .TryDeletePlayer(player);
        if (result == DeleteResult.Success)
        {
            MessageBox.Show("Игрок успешно удален!", "Удаление игрока",
                            MessageBoxButton.OK, MessageBoxImage.Information);
            PlayerDeletedEvent.OnPlayerDeleted(this, new PlayerDeletedEventArgs(player));
            return;
        }

        MessageBox.Show("Не удалось удалить игрока! Возможно игрок уже удален", "Удаление игрока",
                        MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
