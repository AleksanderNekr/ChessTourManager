using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public class AddPlayerCommand : CommandBase
{
    private readonly PlayersViewModel _playersViewModel;

    public AddPlayerCommand(PlayersViewModel viewModel)
    {
        _playersViewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        if (TournamentsListViewModel.SelectedTournament == null)
        {
            return;
        }

        IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext)
                      .TryAddPlayer(out Player? player, TournamentsListViewModel.SelectedTournament.TournamentId,
                                    TournamentsListViewModel.SelectedTournament.OrganizerId,
                                    "", "");
        PlayerAddedEvent.OnPlayerAdded(new PlayerAddedEventArgs(player));

    }
}
