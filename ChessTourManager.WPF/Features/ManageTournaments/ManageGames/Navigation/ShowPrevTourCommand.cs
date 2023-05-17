using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.Navigation;

public class ShowPrevTourCommand : CommandBase
{
    private readonly PairsGridViewModel _pairsGridViewModel;

    public ShowPrevTourCommand(PairsGridViewModel pairsGridViewModel)
    {
        this._pairsGridViewModel = pairsGridViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (this._pairsGridViewModel.SelectedTour > 1)
        {
            this._pairsGridViewModel.SelectedTour--;
        }
    }
}
