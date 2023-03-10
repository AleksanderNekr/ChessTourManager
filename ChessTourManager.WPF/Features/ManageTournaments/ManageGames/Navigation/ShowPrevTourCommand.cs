using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.Navigation;

public class ShowPrevTourCommand : CommandBase
{
    private readonly PairsGridViewModel _pairsGridViewModel;

    public ShowPrevTourCommand(PairsGridViewModel pairsGridViewModel)
    {
        _pairsGridViewModel = pairsGridViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (_pairsGridViewModel.SelectedTour > 1)
        {
            _pairsGridViewModel.SelectedTour--;
        }
    }
}
