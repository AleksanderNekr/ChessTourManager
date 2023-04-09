using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

public class OpenTournamentCommand : CommandBase
{
    private readonly MainViewModel _mainViewModel;

    public OpenTournamentCommand(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is Tournament tournament)
        {
            MainViewModel.SelectedTournament = tournament;
            _mainViewModel.IsOpened          = true;
            TournamentOpenedEvent.OnTournamentOpened(this, new TournamentOpenedEventArgs(tournament));
        }
    }
}
