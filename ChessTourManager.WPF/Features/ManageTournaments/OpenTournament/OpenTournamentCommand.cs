using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

public class OpenTournamentCommand : CommandBase
{
    private readonly TournamentsListViewModel _tournamentsListViewModel;

    public OpenTournamentCommand(TournamentsListViewModel tournamentsListViewModel)
    {
        _tournamentsListViewModel = tournamentsListViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is Tournament tournament)
        {
            TournamentsListViewModel.SelectedTournament = tournament;
            _tournamentsListViewModel.IsOpened          = true;
            TournamentOpenedEvent.OnTournamentOpened(new TournamentOpenedEventArgs(tournament));
        }
    }
}
