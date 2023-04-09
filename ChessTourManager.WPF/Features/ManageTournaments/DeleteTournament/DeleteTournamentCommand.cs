using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Delete;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.DeleteTournament;

public class DeleteTournamentCommand : CommandBase
{
    private readonly MainViewModel _mainViewModel;

    public DeleteTournamentCommand(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    public override void Execute(object? parameter)
    {
        MessageBoxResult questResult = MessageBox.Show("Вы действительно хотите удалить турнир?", "Удаление турнира",
                                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (questResult == MessageBoxResult.No)
        {
            return;
        }

        if (parameter is Tournament tournament)
        {
            DeleteResult result = IDeleteQueries.CreateInstance(TournamentsListViewModel.TournamentsListContext)
                                                .TryDeleteTournament(tournament);

        if (result == DeleteResult.Success)
        {
            _mainViewModel.UpdateTournamentsList();
            MessageBox.Show("Турнир успешно удален!", "Удаление турнира", MessageBoxButton.OK,
                            MessageBoxImage.Information);

                TournamentDeletedEvent.OnTournamentDeleted(new DeleteTournamentEventArgs(tournament));
            }
        }
    }
}
