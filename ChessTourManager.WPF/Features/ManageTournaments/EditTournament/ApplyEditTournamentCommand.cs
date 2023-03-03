using System.Windows;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public class ApplyEditTournamentCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        TournamentsListViewModel.TournamentsListContext.SaveChanges();
        if (EditTournamentViewModel.EditingTournament != null)
        {
            MessageBox.Show("Турнир успешно отредактирован!", "Редактирвонаие турнира",
                            MessageBoxButton.OK, MessageBoxImage.Information);
            TournamentEditedEvent.OnTournamentEdited(new TournamentEditedEventArgs(EditTournamentViewModel
                                                        .EditingTournament));
        }
    }
}
