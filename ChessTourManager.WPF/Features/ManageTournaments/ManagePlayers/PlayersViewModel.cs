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

public class PlayersViewModel : ViewModelBase, IDisposable
{
    internal static ChessTourContext PlayersContext = new();

    private AddPlayerCommand?         _addPlayerCommand;
    private DeletePlayerCommand?      _deletePlayerCommand;
    private ExportPlayersListCommand? _exportPlayersListCommand;
    private PrintPlayersListCommand?  _printPlayersListCommand;

    private ObservableCollection<Group>?  _groupsAvailable;
    private ObservableCollection<Player>? _playersCollection;
    private ObservableCollection<Team>?   _teamsAvailable;

    private bool _arePlayersUpdating;

    public PlayersViewModel()
    {
        this.Subscribe();
    }

    public ObservableCollection<Player>? PlayersCollection
    {
        get
        {
            if (this._playersCollection is not null)
            {
                return this._playersCollection;
            }

            if (MainViewModel.SelectedTournament == null)
            {
                return new ObservableCollection<Player>();
            }

            return this._playersCollection;
        }
        private set { this.SetField(ref this._playersCollection, value); }
    }

    public ICommand AddPlayerCommand
    {
        get { return this._addPlayerCommand ??= new AddPlayerCommand(); }
    }

    public ICommand DeletePlayerCommand
    {
        get { return this._deletePlayerCommand ??= new DeletePlayerCommand(); }
    }

    public ObservableCollection<Team>? TeamsAvailable
    {
        get
        {
            if (this._teamsAvailable is not null)
            {
                return this._teamsAvailable;
            }

            this.UpdateAvailableTeams();
            return this._teamsAvailable;
        }
        private set { this.SetField(ref this._teamsAvailable, value); }
    }

    public ObservableCollection<int> BirthYears
    {
        get { return new ObservableCollection<int>(Enumerable.Range(DateTime.UtcNow.Year - 100, 100)); }
    }

    public ObservableCollection<Group>? GroupsAvailable
    {
        get
        {
            if (this._groupsAvailable is not null)
            {
                return this._groupsAvailable;
            }

            this.UpdateAvailableGroups();
            return this._groupsAvailable;
        }
        private set { this.SetField(ref this._groupsAvailable, value); }
    }

    public ICommand ExportPlayersListCommand
    {
        get { return this._exportPlayersListCommand ??= new ExportPlayersListCommand(); }
    }

    public ICommand PrintPlayersListCommand
    {
        get { return this._printPlayersListCommand ??= new PrintPlayersListCommand(); }
    }

    private void GroupDeletedEvent_GroupDeleted(object source, GroupDeletedEventArgs groupDeletedEventArgs)
    {
        this.UpdateAvailableGroups();
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        if (source != this)
        {
            this.UpdateAvailableGroups();
            this.UpdatePlayers();
        }
    }

    private void GroupAddedEvent_GroupAdded(object source, GroupAddedEventArgs groupAddedEventArgs)
    {
        this.UpdateAvailableGroups();
    }

    private void TeamDeletedEvent_TeamDeleted(object source, TeamDeletedEventArgs teamDeletedEventArgs)
    {
        this.UpdateAvailableTeams();
    }

    private void TeamEditedEventTeamEdited(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        if (source != this)
        {
            this.UpdateAvailableTeams();
            this.UpdatePlayers();
        }
    }

    private void TeamAddedEvent_TeamAdded(object source, TeamAddedEventArgs teamAddedEventArgs)
    {
        this.UpdateAvailableTeams();
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

            this.UpdatePlayers();
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
                   .TryGetTeamsWithPlayers(LoginViewModel.CurrentUser.Id,
                                           MainViewModel.SelectedTournament.TournamentId,
                                           out List<Team>? teams);

        this.TeamsAvailable = new ObservableCollection<Team>(teams ?? Enumerable.Empty<Team>());
    }

    private void UpdateAvailableGroups()
    {
        if (LoginViewModel.CurrentUser is null || MainViewModel.SelectedTournament is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetGroups(LoginViewModel.CurrentUser.Id,
                                 MainViewModel.SelectedTournament.TournamentId,
                                 out List<Group>? groups);

        this.GroupsAvailable = new ObservableCollection<Group>(groups ?? Enumerable.Empty<Group>());
    }

    private void TournamentOpenedEvent_TournamentOpened(object                    source,
                                                        TournamentOpenedEventArgs tournamentOpenedEventArgs)
    {
        PlayersContext        = new ChessTourContext();
        this._arePlayersUpdating = true;
        this.UpdateAvailableTeams();
        this.UpdateAvailableGroups();
        this.UpdatePlayers();
        this._arePlayersUpdating = false;
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened += this.TournamentOpenedEvent_TournamentOpened;

        TeamAddedEvent.TeamAdded     += this.TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited  += this.TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted += this.TeamDeletedEvent_TeamDeleted;

        GroupAddedEvent.GroupAdded     += this.GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged += this.GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted += this.GroupDeletedEvent_GroupDeleted;

        PlayerAddedEvent.PlayerAdded     += this.PlayerAddedEvent_PlayerAdded;
        PlayerEditedEvent.PlayerEdited   += this.PlayerEditedEvent_PlayerEdited;
        PlayerDeletedEvent.PlayerDeleted += this.PlayerDeletedEvent_PlayerDeleted;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened -= this.TournamentOpenedEvent_TournamentOpened;

        GroupAddedEvent.GroupAdded     -= this.GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged -= this.GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted -= this.GroupDeletedEvent_GroupDeleted;

        TeamAddedEvent.TeamAdded     -= this.TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited  -= this.TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted -= this.TeamDeletedEvent_TeamDeleted;

        PlayerAddedEvent.PlayerAdded     -= this.PlayerAddedEvent_PlayerAdded;
        PlayerEditedEvent.PlayerEdited   -= this.PlayerEditedEvent_PlayerEdited;
        PlayerDeletedEvent.PlayerDeleted -= this.PlayerDeletedEvent_PlayerDeleted;
    }

    private void PlayerDeletedEvent_PlayerDeleted(object source, PlayerDeletedEventArgs e)
    {
        this.UpdatePlayers();
    }

    private void PlayerAddedEvent_PlayerAdded(object source, PlayerAddedEventArgs e)
    {
        this.UpdatePlayers();
    }

    private void PlayerEditedEvent_PlayerEdited(object source, PlayerEditedEventArgs e)
    {
        if (source != this)
        {
            this.UpdatePlayers();
        }
    }

    private void UpdatePlayers()
    {
        if (LoginViewModel.CurrentUser is null || MainViewModel.SelectedTournament is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetPlayersWithTeamsAndGroups(LoginViewModel.CurrentUser.Id,
                                                    MainViewModel.SelectedTournament.TournamentId,
                                                    out List<Player>? players);

        if (players is not null)
        {
            this.PlayersCollection = new ObservableCollection<Player>(players);
        }


        if (this.PlayersCollection != null)
        {
            foreach (Player player in this.PlayersCollection)
            {
                player.PropertyChanged -= this.Player_PropertyChanged;
                player.PropertyChanged += this.Player_PropertyChanged;
            }
        }
    }

    private void Player_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (this._arePlayersUpdating || !this.TrySaveChanges())
        {
            return;
        }

        PlayerEditedEvent.OnPlayerEdited(this, new PlayerEditedEventArgs(sender as Player));
        if (e.PropertyName == nameof(Player.Team))
        {
            TeamChangedEvent.OnTeamChanged(this, new TeamChangedEventArgs((sender as Player)?.Team));
        }
        else if (e.PropertyName == nameof(Player.Group))
        {
            GroupChangedEvent.OnGroupChanged(this, new GroupChangedEventArgs((sender as Player)?.Group));
        }
    }

    public void Dispose()
    {
        this.Unsubscribe();
    }
}
