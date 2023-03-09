using System.Collections.ObjectModel;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageRatings;

public class ManageRatingsViewModel : ViewModelBase
{
    internal static readonly ChessTourContext RatingsContext = new();

    private ObservableCollection<Player>? _playersSorted;

    public ManageRatingsViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    public ObservableCollection<Player>? PlayersSorted
    {
        get { return _playersSorted; }
        private set { SetField(ref _playersSorted, value); }
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        IGetQueries.CreateInstance(RatingsContext)
                   .TryGetPlayersWithTeamsAndGroups(LoginViewModel.CurrentUser!.UserId,
                                  TournamentsListViewModel.SelectedTournament!.TournamentId,
                                  out IQueryable<Player>? players);

        // Sort players descending by PointsCount, RatioSum1 and RatioSum2
        IOrderedQueryable<Player>? playersSorted = players?.OrderByDescending(p => p.PointsCount)
                                                           .ThenByDescending(p => p.RatioSum1)
                                                           .ThenByDescending(p => p.RatioSum2);


        if (playersSorted != null)
        {
            PlayersSorted = new ObservableCollection<Player>(playersSorted);
        }
    }
}
