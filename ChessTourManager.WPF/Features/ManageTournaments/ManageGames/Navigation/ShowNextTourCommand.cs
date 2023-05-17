using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.Navigation;

public class ShowNextTourCommand : CommandBase
{
    private readonly PairsGridViewModel _pairsGridViewModel;

    public ShowNextTourCommand(PairsGridViewModel pairsGridViewModel)
    {
        this._pairsGridViewModel = pairsGridViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (this._pairsGridViewModel.SelectedTour < this._pairsGridViewModel.CurrentTour)
        {
            this._pairsGridViewModel.SelectedTour++;
        }
    }
}
