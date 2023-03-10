using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageRatings;

public class ManageRatingsViewModel : ViewModelBase
{
    private static readonly ChessTourContext RatingsContext = PlayersViewModel.PlayersContext;

    private ObservableCollection<Player>? _playersSorted;
    private string?                       _title;

    public ManageRatingsViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    private void TourAddedEvent_TourAdded(object sender, TourAddedEventArgs e)
    {
        Title = $"Рейтинг-лист после {e.TourNumber} тура";
    }

    private void ResultChangedEvent_ResultChanged(object? sender, ResultChangedEventArgs e)
    {
        UpdateRating();
    }

    private void PlayerEditedEvent_PlayerEdited(PlayerEditedEventArgs e)
    {
        UpdateRating();
    }

    private void GroupDeletedEvent_GroupDeleted(GroupDeletedEventArgs e)
    {
        UpdateRating();
    }

    private void GroupChangedEvent_GroupChanged(GroupChangedEventArgs e)
    {
        UpdateRating();
    }

    private void TeamDeletedEvent_TeamDeleted(TeamDeletedEventArgs e)
    {
        UpdateRating();
    }

    private void TeamChangedEvent_TeamChanged(TeamChangedEventArgs e)
    {
        UpdateRating();
    }

    private void PlayerDeletedEvent_PlayerDeleted(PlayerDeletedEventArgs e)
    {
        UpdateRating();
    }

    private void PlayerAddedEvent_PlayerAdded(PlayerAddedEventArgs e)
    {
        UpdateRating();
    }

    private void TournamentEditedEvent_TournamentEdited(TournamentEditedEventArgs e)
    {
        UpdateRating();
    }

    public ObservableCollection<Player>? PlayersSorted
    {
        get
        {
            if (_playersSorted is null)
            {
                UpdateRating();
            }

            return _playersSorted;
        }
        private set { SetField(ref _playersSorted, value); }
    }

    public string Title
    {
        get { return _title ?? string.Empty; }
        private set { SetField(ref _title, value); }
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        UpdateRating();

        IGetQueries.CreateInstance(RatingsContext)
                   .TryGetGames(LoginViewModel.CurrentUser!.UserId,
                                TournamentsListViewModel.SelectedTournament!.TournamentId,
                                out IEnumerable<Game>? games);
        // Get the number of the last tour.
        int lastTourNumber = games?.Max(g => g.TourNumber) ?? 0;

        Title = $"Рейтинг-лист после {lastTourNumber} тура";

        TournamentEditedEvent.TournamentEdited += TournamentEditedEvent_TournamentEdited;

        PlayerAddedEvent.PlayerAdded     += PlayerAddedEvent_PlayerAdded;
        PlayerEditedEvent.PlayerEdited   += PlayerEditedEvent_PlayerEdited;
        PlayerDeletedEvent.PlayerDeleted += PlayerDeletedEvent_PlayerDeleted;

        TeamChangedEvent.TeamChanged += TeamChangedEvent_TeamChanged;
        TeamDeletedEvent.TeamDeleted += TeamDeletedEvent_TeamDeleted;

        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted += GroupDeletedEvent_GroupDeleted;

        ResultChangedEvent.ResultChanged += ResultChangedEvent_ResultChanged;

        TourAddedEvent.TourAdded += TourAddedEvent_TourAdded;
    }

    private void UpdateRating()
    {
        if (TournamentsListViewModel.SelectedTournament == null)
        {
            return;
        }

        IGetQueries.CreateInstance(RatingsContext)
                   .TryGetPlayersWithTeamsAndGroups(LoginViewModel.CurrentUser!.UserId,
                                                    TournamentsListViewModel.SelectedTournament.TournamentId,
                                                    out IEnumerable<Player>? players);

        // Sort players descending by PointsCount, RatioSum1 and RatioSum2.
        IOrderedEnumerable<Player>? playersSorted = players?.OrderByDescending(p => p.PointsCount)
                                                            .ThenByDescending(p => p.RatioSum1)
                                                            .ThenByDescending(p => p.RatioSum2);


        if (playersSorted != null)
        {
            PlayersSorted = new ObservableCollection<Player>(playersSorted);
        }
    }
}
