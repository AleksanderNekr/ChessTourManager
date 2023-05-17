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
        this._mainViewModel = mainViewModel;
    }

    public override void Execute(object? parameter)
    {
        MessageBoxResult boxResult = MessageBox.Show("Вы действительно хотите удалить турнир?", "Удаление турнира",
                                                     MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (boxResult == MessageBoxResult.No || parameter is not Tournament tournament)
        {
            return;
        }

        DeleteResult result = IDeleteQueries.CreateInstance(MainViewModel.MainContext)
                                            .TryDeleteTournament(tournament);

        if (result == DeleteResult.Success)
        {
            this._mainViewModel.UpdateTournamentsList();
            MessageBox.Show("Турнир успешно удален!", "Удаление турнира", MessageBoxButton.OK,
                            MessageBoxImage.Information);

            TournamentDeletedEvent.OnTournamentDeleted(this, new DeleteTournamentEventArgs(tournament));
            return;
        }

        MessageBox.Show("Не удалось удалить турнир! Возможно турнир уже удален", "Удаление турнира",
                        MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
