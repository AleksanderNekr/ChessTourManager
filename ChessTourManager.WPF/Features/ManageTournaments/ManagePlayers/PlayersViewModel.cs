using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
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
    internal static ChessTourContext             PlayersContext = new();
    private         ICommand?                    _addPlayerCommand;
    private         ICommand?                    _deletePlayerCommand;
    private         ExportPlayersListCommand?    _exportPlayersListCommand;
    private         ObservableCollection<Group>? _groupsAvailable;

    private ObservableCollection<Player>? _playersCollection;
    private PrintPlayersListCommand?      _printPlayersListCommand;
    private ObservableCollection<Team>?   _teamsAvailable;

    public PlayersViewModel()
    {
        TournamentOpenedEvent.TournamentOpened -= TournamentOpenedEvent_TournamentOpened;
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    public ObservableCollection<Player>? PlayersCollection
    {
        get
        {
            if (_playersCollection is { })
            {
                return _playersCollection;
            }

            if (MainViewModel.SelectedTournament == null)
            {
                return new ObservableCollection<Player>();
            }

            return _playersCollection;
        }
        private set { SetField(ref _playersCollection, value); }
    }

    public ICommand AddPlayerCommand
    {
        get { return _addPlayerCommand ??= new AddPlayerCommand(); }
    }

    public ICommand DeletePlayerCommand
    {
        get { return _deletePlayerCommand ??= new DeletePlayerCommand(); }
    }

    public ObservableCollection<Team>? TeamsAvailable
    {
        get
        {
            if (_teamsAvailable is { })
            {
                return _teamsAvailable;
            }

            UpdateAvailableTeams();
            return _teamsAvailable;
        }
        set { SetField(ref _teamsAvailable, value); }
    }

    public ObservableCollection<int> BirthYears
    {
        get { return new ObservableCollection<int>(Enumerable.Range(DateTime.UtcNow.Year - 100, 100)); }
    }

    public ObservableCollection<Group>? GroupsAvailable
    {
        get
        {
            if (_groupsAvailable is { })
            {
                return _groupsAvailable;
            }

            UpdateAvailableGroups();
            return _groupsAvailable;
        }
        set { SetField(ref _groupsAvailable, value); }
    }

    public ICommand ExportPlayersListCommand
    {
        get { return _exportPlayersListCommand ??= new ExportPlayersListCommand(); }
    }

    public ICommand PrintPlayersListCommand
    {
        get { return _printPlayersListCommand ??= new PrintPlayersListCommand(); }
    }

    private void GroupDeletedEvent_GroupDeleted(object source, GroupDeletedEventArgs groupDeletedEventArgs)
    {
        UpdateAvailableGroups();
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        if (source != this)
        {
            UpdateAvailableGroups();
            UpdatePlayers();
        }
    }

    private void GroupAddedEvent_GroupAdded(object source, GroupAddedEventArgs groupAddedEventArgs)
    {
        UpdateAvailableGroups();
    }

    private void TeamDeletedEvent_TeamDeleted(object source, TeamDeletedEventArgs teamDeletedEventArgs)
    {
        UpdateAvailableTeams();
    }

    private void TeamEditedEventTeamEdited(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        if (source != this)
        {
            UpdateAvailableTeams();
            UpdatePlayers();
        }
    }

    private void TeamAddedEvent_TeamAdded(object source, TeamAddedEventArgs teamAddedEventArgs)
    {
        UpdateAvailableTeams();
    }

    private bool TrySaveChanges()
    {
        try
        {
            PlayersContext.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            MessageBox.Show("Возможно игрок с такими параметрами уже существует.",
                            "Ошибка сохранения",
                            MessageBoxButton.OK, MessageBoxImage.Error);

            PlayersContext.ChangeTracker
                          .Entries()
                          .ToList()
                          .ForEach(entry => entry.State = EntityState.Unchanged);

            UpdatePlayers();
            return false;
        }
    }

    private void UpdateAvailableTeams()
    {
        if (LoginViewModel.CurrentUser is null || MainViewModel.SelectedTournament is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetTeamsWithPlayers(LoginViewModel.CurrentUser.UserId,
                                           MainViewModel.SelectedTournament.TournamentId,
                                           out List<Team>? teams);

        TeamsAvailable = new ObservableCollection<Team>(teams ?? Enumerable.Empty<Team>());
    }

    private void UpdateAvailableGroups()
    {
        if (LoginViewModel.CurrentUser is null || MainViewModel.SelectedTournament is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetGroups(LoginViewModel.CurrentUser.UserId,
                                 MainViewModel.SelectedTournament.TournamentId,
                                 out List<Group>? groups);

        GroupsAvailable = new ObservableCollection<Group>(groups ?? Enumerable.Empty<Group>());
    }

    private void TournamentOpenedEvent_TournamentOpened(object                    source,
                                                        TournamentOpenedEventArgs tournamentOpenedEventArgs)
    {
        TeamAddedEvent.TeamAdded     += TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited  += TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted += TeamDeletedEvent_TeamDeleted;

        GroupAddedEvent.GroupAdded     += GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted += GroupDeletedEvent_GroupDeleted;
        UpdatePlayers();
        if (_playersCollection == null)
        {
            return;
        }

        foreach (Player player in _playersCollection)
        {
            player.PropertyChanged -= Player_PropertyChanged;
            player.PropertyChanged += Player_PropertyChanged;
        }
    }

    private void UpdatePlayers()
    {
        if (LoginViewModel.CurrentUser is null || MainViewModel.SelectedTournament is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetPlayersWithTeamsAndGroups(LoginViewModel.CurrentUser.UserId,
                                                    MainViewModel.SelectedTournament.TournamentId,
                                                    out List<Player>? players);

        if (players is { })
        {
            PlayersCollection = new ObservableCollection<Player>(players);
        }
    }

    private void Player_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (TrySaveChanges())
        {
            PlayerEditedEvent.OnPlayerEdited(this, new PlayerEditedEventArgs(sender as Player));
            TeamChangedEvent.OnTeamChanged(this, new TeamChangedEventArgs((sender as Player)?.Team));
            GroupChangedEvent.OnGroupChanged(this, new GroupChangedEventArgs((sender as Player)?.Group));
        }
    }
}
