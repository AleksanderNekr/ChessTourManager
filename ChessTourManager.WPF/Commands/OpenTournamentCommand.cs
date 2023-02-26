using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.ViewModels;

namespace ChessTourManager.WPF.Commands;

public class OpenTournamentCommand : CommandBase
{
    private readonly TournamentsListViewModel _tournamentsListViewModel;

    public OpenTournamentCommand(TournamentsListViewModel tournamentsListViewModel) =>
        _tournamentsListViewModel = tournamentsListViewModel;

    public override void Execute(object? parameter)
    {
        if (parameter is Tournament tournament)
        {
            _tournamentsListViewModel.SelectedTournament = tournament;
            _tournamentsListViewModel.IsOpened           = true;
        }
    }
}
