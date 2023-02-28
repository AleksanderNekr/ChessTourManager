using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries;
using ChessTourManager.WPF.Commands.Events;
using ChessTourManager.WPF.ViewModels;

namespace ChessTourManager.WPF.Commands;

public class DeletePlayerCommand : CommandBase
{
    private readonly PlayersViewModel _playersViewModel;

    public DeletePlayerCommand(PlayersViewModel playersViewModel) => _playersViewModel = playersViewModel;

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
        }
    }
}
