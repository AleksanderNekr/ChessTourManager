using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public class PlayersViewModel : ViewModelBase
{
    internal static readonly ChessTourContext             PlayersContext = new();
    private                  ICommand?                    _addPlayerCommand;
    private                  ICommand?                    _deletePlayerCommand;
    private                  ObservableCollection<Group>? _groupsAvailable;

    private ObservableCollection<Player>? _playersCollection;
    private ObservableCollection<Team>?   _teamsAvailable;

    public PlayersViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

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
        private set { SetField(ref _playersCollection, value); }
    }

    public ICommand AddPlayerCommand
    {
        get { return _addPlayerCommand ??= new AddPlayerCommand(this); }
    }

    public ICommand DeletePlayerCommand
    {
        get { return _deletePlayerCommand ??= new DeletePlayerCommand(); }
    }

    public ObservableCollection<Team> TeamsAvailable
    {
        get
        {
            if (_teamsAvailable is not null)
            {
                return _teamsAvailable;
            }

            UpdateTeams();

            return _teamsAvailable!;
        }
        set { SetField(ref _teamsAvailable, value); }
    }

    public ObservableCollection<int> BirthYears
    {
        get { return new ObservableCollection<int>(Enumerable.Range(DateTime.UtcNow.Year - 100, 100)); }
    }

    public ObservableCollection<Group> GroupsAvailable
    {
        get
        {
            if (_groupsAvailable is not null)
            {
                return _groupsAvailable;
            }

            UpdateGroups();

            return _groupsAvailable!;
        }
        set { SetField(ref _groupsAvailable, value); }
    }

    private void TournamentEditedEvent_TournamentEdited(TournamentEditedEventArgs e)
    {
        UpdatePlayers();
    }

    private void PlayerEditedEvent_PlayerEdited(PlayerEditedEventArgs e)
    {
        UpdatePlayers();
    }

    private void GroupDeletedEvent_GroupDeleted(GroupDeletedEventArgs e)
    {
        UpdateGroups();
    }

    private void GroupChangedEvent_GroupChanged(GroupChangedEventArgs e)
    {
        UpdateGroups();
    }

    private void GroupAddedEvent_GroupAdded(GroupAddedEventArgs e)
    {
        UpdateGroups();
    }

    private void TeamDeletedEvent_TeamDeleted(TeamDeletedEventArgs e)
    {
        UpdateTeams();
    }

    private void TeamEditedEventTeamEdited(TeamChangedEventArgs e)
    {
        UpdateTeams();
    }

    private void TeamAddedEvent_TeamAdded(TeamAddedEventArgs e)
    {
        UpdateTeams();
    }


    public static void TrySavePlayers()
    {
        try
        {
            PlayersContext.ChangeTracker.DetectChanges();

            // If there are no changes, don't try to save.
            if (PlayersContext.ChangeTracker.HasChanges())
            {
                PlayersContext.SaveChanges();
                PlayerEditedEvent.OnPlayerEdited(new PlayerEditedEventArgs(null!));
            }
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Возможно игрок с такими параметрами уже существует.", "Ошибка сохранения",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            // Undo changes.
            PlayersContext.ChangeTracker
                          .Entries()
                          .ToList()
                          .ForEach(entry => { entry.State = EntityState.Unchanged; });
        }
    }

    private void UpdateTeams()
    {
        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetTeamsWithPlayers(LoginViewModel.CurrentUser!.UserId,
                                           TournamentsListViewModel.SelectedTournament!.TournamentId,
                                           out IEnumerable<Team>? teams);

        SetField(ref _teamsAvailable, new ObservableCollection<Team>(teams ?? Enumerable.Empty<Team>()));
    }

    private void UpdateGroups()
    {
        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetGroups(LoginViewModel.CurrentUser!.UserId,
                                 TournamentsListViewModel.SelectedTournament!.TournamentId,
                                 out IEnumerable<Group>? groups);

        SetField(ref _groupsAvailable, new ObservableCollection<Group>(groups ?? Enumerable.Empty<Group>()));
    }

    private void PlayerDeletedEvent_PlayerDeleted(PlayerDeletedEventArgs e)
    {
        UpdatePlayers();
    }

    private void PlayerAddedEvent_PlayerAdded(PlayerAddedEventArgs e)
    {
        UpdatePlayers();
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        TournamentEditedEvent.TournamentEdited += TournamentEditedEvent_TournamentEdited;

        PlayerAddedEvent.PlayerAdded     += PlayerAddedEvent_PlayerAdded;
        PlayerEditedEvent.PlayerEdited   += PlayerEditedEvent_PlayerEdited;
        PlayerDeletedEvent.PlayerDeleted += PlayerDeletedEvent_PlayerDeleted;

        TeamAddedEvent.TeamAdded     += TeamAddedEvent_TeamAdded;
        TeamEditedEvent.TeamEdited += TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted += TeamDeletedEvent_TeamDeleted;

        GroupAddedEvent.GroupAdded     += GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted += GroupDeletedEvent_GroupDeleted;
        UpdatePlayers();
    }

    private void UpdatePlayers()
    {
        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetPlayersWithTeamsAndGroups(LoginViewModel.CurrentUser!.UserId,
                                                    TournamentsListViewModel.SelectedTournament!.TournamentId,
                                                    out IEnumerable<Player>? players);

        if (players != null)
        {
            PlayersCollection = new ObservableCollection<Player>(players);
        }
    }
}
