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

        DeleteResult response = IDeleteQueries.CreateInstance(PlayersViewModel.PlayersContext)
                                              .TryDeletePlayer(player);
        if (response == DeleteResult.Success)
        {
            PlayerDeletedEvent.OnPlayerDeleted(new PlayerDeletedEventArgs(player));

            MessageBox.Show("Игрок успешно удален!", "Удаление игрока",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
