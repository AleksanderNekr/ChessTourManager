using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageRatings;

public class ManageRatingsViewModel : ViewModelBase
{
    private static readonly ChessTourContext RatingsContext = PlayersViewModel.PlayersContext;

    private ObservableCollection<Player>? _playersSorted;

    public ManageRatingsViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        TournamentEditedEvent.TournamentEdited += TournamentEditedEvent_TournamentEdited;

        PlayerAddedEvent.PlayerAdded     += PlayerAddedEvent_PlayerAdded;
        PlayerEditedEvent.PlayerEdited   += PlayerEditedEvent_PlayerEdited;
        PlayerDeletedEvent.PlayerDeleted += PlayerDeletedEvent_PlayerDeleted;

        TeamAddedEvent.TeamAdded     += TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamChanged += TeamChangedEvent_TeamChanged;
        TeamDeletedEvent.TeamDeleted += TeamDeletedEvent_TeamDeleted;

        GroupAddedEvent.GroupAdded     += GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted += GroupDeletedEvent_GroupDeleted;

        ResultChangedEvent.ResultChanged += ResultChangedEvent_ResultChanged;
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

    private void GroupAddedEvent_GroupAdded(GroupAddedEventArgs e)
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

    private void TeamAddedEvent_TeamAdded(TeamAddedEventArgs e)
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
        get { return _playersSorted; }
        private set { SetField(ref _playersSorted, value); }
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        UpdateRating();
    }

    private void UpdateRating()
    {
        IGetQueries.CreateInstance(RatingsContext)
                   .TryGetPlayersWithTeamsAndGroups(LoginViewModel.CurrentUser!.UserId,
                                                    TournamentsListViewModel.SelectedTournament!.TournamentId,
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
