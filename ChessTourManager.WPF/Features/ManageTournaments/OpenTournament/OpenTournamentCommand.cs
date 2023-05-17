using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

public class OpenTournamentCommand : CommandBase
{
    private readonly MainViewModel _mainViewModel;

    public OpenTournamentCommand(MainViewModel mainViewModel)
    {
        this._mainViewModel = mainViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is not Tournament tournament)
        {
            return;
        }

        if (MainViewModel.SelectedTournament == null || !MainViewModel.SelectedTournament.Equals(tournament))
        {
            MainViewModel.SelectedTournament = tournament;
            TournamentOpenedEvent.OnTournamentOpened(this, new TournamentOpenedEventArgs(tournament));
        }

        this._mainViewModel.IsOpened = true;
    }
}
