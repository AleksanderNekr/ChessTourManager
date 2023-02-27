using System.Collections.ObjectModel;
using System.Linq;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries;
using ChessTourManager.WPF.Commands.Events;

namespace ChessTourManager.WPF.ViewModels;

public class PlayersViewModel : ViewModelBase
{
    public PlayersViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        IGetQueries.CreateInstance()
                   .TryGetPlayers(LoginViewModel.CurrentUser.UserId,
                                  TournamentsListViewModel.SelectedTournament.TournamentId,
                                  out IQueryable<Player>? players);

        if (players != null)
        {
            PlayersCollection = new ObservableCollection<Player>(players);
        }
    }

    private ObservableCollection<Player>? _playersCollection;

    public ObservableCollection<Player>? PlayersCollection
    {
        get
        {
            if (_playersCollection != null)
            {
                return _playersCollection;
            }

            if (TournamentsListViewModel.SelectedTournament == null)
            {
                return new ObservableCollection<Player>();
            }

            return _playersCollection;
        }
        set => SetField(ref _playersCollection, value);
    }
}
