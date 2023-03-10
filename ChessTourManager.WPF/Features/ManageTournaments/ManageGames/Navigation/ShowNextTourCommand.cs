using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.Navigation;

public class ShowNextTourCommand : CommandBase
{
    private readonly PairsGridViewModel _pairsGridViewModel;

    public ShowNextTourCommand(PairsGridViewModel pairsGridViewModel)
    {
        _pairsGridViewModel = pairsGridViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (_pairsGridViewModel.SelectedTour < _pairsGridViewModel.CurrentTour)
        {
            _pairsGridViewModel.SelectedTour++;
        }
    }
}
