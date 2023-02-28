using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries;
using ChessTourManager.WPF.Commands.Events;
using ChessTourManager.WPF.ViewModels;

namespace ChessTourManager.WPF.Commands;

public class AddPlayerCommand : CommandBase
{
    private readonly PlayersViewModel _playersViewModel;

    public AddPlayerCommand(PlayersViewModel viewModel)
    {
        _playersViewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext)
                      .TryAddPlayer(out Player? player, TournamentsListViewModel.SelectedTournament.TournamentId,
                                    TournamentsListViewModel.SelectedTournament.OrganizerId,
                                    "", "");
        PlayerAddedEvent.OnPlayerAdded(new PlayerAddedEventArgs(player));
    }
}
