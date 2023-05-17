using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;
using ChessTourManager.WPF.Features.ManageTournaments.DeleteTournament;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.Tree;

public class TreeViewModel : ViewModelBase, IDisposable
{
    private static readonly ChessTourContext TreeContext = MainViewModel.MainContext;
    private                 Player?          _selectedPlayer;
    private                 Team?            _selectedTeam;
    private                 Tournament?      _selectedTournament;

    private ObservableCollection<Tournament>? _tournaments;

    public TreeViewModel()
    {
        this.Subscribe();
    }

    private void Subscribe()
    {
        TournamentEditedEvent.TournamentEdited   += this.TournamentEditedEvent_TournamentEdited;
        TournamentCreatedEvent.TournamentCreated += this.TournamentCreatedEvent_TournamentCreated;
        TournamentDeletedEvent.TournamentDeleted += this.TournamentDeletedEvent_TournamentDeleted;

        TeamAddedEvent.TeamAdded     += this.TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited  += this.TeamChangedEvent_TeamChanged;
        TeamDeletedEvent.TeamDeleted += this.TeamChangedEvent_TeamDeleted;

        PlayerAddedEvent.PlayerAdded     += this.PlayerChangedEvent_PlayerAdded;
        PlayerDeletedEvent.PlayerDeleted += this.PlayerChangedEvent_PlayerDeleted;
        PlayerEditedEvent.PlayerEdited   += this.PlayerChangedEvent_PlayerEdited;
    }

    private void Unsubscribe()
    {
        TournamentEditedEvent.TournamentEdited   -= this.TournamentEditedEvent_TournamentEdited;
        TournamentCreatedEvent.TournamentCreated -= this.TournamentCreatedEvent_TournamentCreated;
        TournamentDeletedEvent.TournamentDeleted -= this.TournamentDeletedEvent_TournamentDeleted;

        TeamAddedEvent.TeamAdded     -= this.TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited  -= this.TeamChangedEvent_TeamChanged;
        TeamDeletedEvent.TeamDeleted -= this.TeamChangedEvent_TeamDeleted;

        PlayerAddedEvent.PlayerAdded     -= this.PlayerChangedEvent_PlayerAdded;
        PlayerDeletedEvent.PlayerDeleted -= this.PlayerChangedEvent_PlayerDeleted;
        PlayerEditedEvent.PlayerEdited   -= this.PlayerChangedEvent_PlayerEdited;
    }

    public ObservableCollection<Tournament>? TournamentsRoot
    {
        get
        {
            if (this._tournaments is not null || LoginViewModel.CurrentUser == null)
            {
                return this._tournaments;
            }

            this.UpdateTournaments();

            return this._tournaments;
        }
        set { this.SetField(ref this._tournaments, value); }
    }

    public Tournament SelectedTournament
    {
        get { return this._selectedTournament ??= new Tournament(); }
        set { this.SetField(ref this._selectedTournament, value); }
    }

    public Team? SelectedTeam
    {
        get { return this._selectedTeam ??= new Team(); }
        set { this.SetField(ref this._selectedTeam, value); }
    }

    public Player? SelectedPlayer
    {
        get { return this._selectedPlayer ??= new Player(); }
        set { this.SetField(ref this._selectedPlayer, value); }
    }

    private void PlayerChangedEvent_PlayerEdited(object source, PlayerEditedEventArgs playerEditedEventArgs)
    {
        this.UpdateTournaments();
    }

    private void PlayerChangedEvent_PlayerDeleted(object source, PlayerDeletedEventArgs playerDeletedEventArgs)
    {
        this.UpdateTournaments();
    }

    private void PlayerChangedEvent_PlayerAdded(object source, PlayerAddedEventArgs playerAddedEventArgs)
    {
        this.UpdateTournaments();
    }

    private void TeamChangedEvent_TeamDeleted(object source, TeamDeletedEventArgs teamDeletedEventArgs)
    {
        this.UpdateTournaments();
    }

    private void TeamChangedEvent_TeamChanged(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        this.UpdateTournaments();
    }

    private void TournamentDeletedEvent_TournamentDeleted(object                    source,
                                                          DeleteTournamentEventArgs deleteTournamentEventArgs)
    {
        this.UpdateTournaments();
    }

    private void TournamentCreatedEvent_TournamentCreated(object                     source,
                                                          TournamentCreatedEventArgs tournamentCreatedEventArgs)
    {
        this.UpdateTournaments();
    }

    private void TeamAddedEvent_TeamAdded(object source, TeamAddedEventArgs teamAddedEventArgs)
    {
        this.UpdateTournaments();
    }

    private void TournamentEditedEvent_TournamentEdited(object                    source,
                                                        TournamentEditedEventArgs tournamentEditedEventArgs)
    {
        this.UpdateTournaments();
    }

    private void UpdateTournaments()
    {
        if (LoginViewModel.CurrentUser is null)
        {
            return;
        }

        IGetQueries.CreateInstance(TreeContext)
                   .TryGetTournamentsWithTeamsAndPlayers(LoginViewModel.CurrentUser.Id,
                                                         out List<Tournament?>? tournaments);


        if (tournaments is not null)
        {
            this.SetField(ref this._tournaments, new ObservableCollection<Tournament>(tournaments), nameof(this.TournamentsRoot));
        }
    }

    public void Dispose()
    {
        this.Unsubscribe();
    }
}
