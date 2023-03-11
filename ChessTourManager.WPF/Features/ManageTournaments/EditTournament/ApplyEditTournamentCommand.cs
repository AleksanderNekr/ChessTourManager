using System;
using System.Windows;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public class ApplyEditTournamentCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите подтвердить редактирование турнира?",
                                                  "Редактирование турнира",
                                                  MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (EditTournamentViewModel.EditingTournament == null || result != MessageBoxResult.Yes)
        {
            return;
        }

        EditTournamentViewModel.EditingTournament.DateLastChange = DateOnly.FromDateTime(DateTime.UtcNow);
        EditTournamentViewModel.EditingTournament.TimeLastChange = TimeOnly.FromDateTime(DateTime.UtcNow);
        TournamentsListViewModel.TournamentsListContext.SaveChanges();

        MessageBox.Show("Турнир успешно отредактирован!", "Редактирование турнира",
                        MessageBoxButton.OK, MessageBoxImage.Information);

        TournamentEditedEvent.OnTournamentEdited(new TournamentEditedEventArgs(EditTournamentViewModel
                                                    .EditingTournament));
    }
}
