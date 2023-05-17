using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;

public class ManageTeamsViewModel : ViewModelBase, IDisposable
{
    private string? _teamName;

    private AddTeamCommand?             _addTeamCommand;
    private ObservableCollection<Team>? _teamsWithPlayers;
    private CompleteAddTeamCommand?     _completeAddTeam;
    private DeleteTeamCommand?          _deleteTeamCommand;
    private EditTeamCommand?            _editTeamCommand;

    public ManageTeamsViewModel()
    {
        this.Subscribe();
    }

    public ObservableCollection<Team>? TeamsWithPlayers
    {
        get { return this._teamsWithPlayers; }
        private set { this.SetField(ref this._teamsWithPlayers, value); }
    }

    public ICommand AddTeamCommand
    {
        get { return this._addTeamCommand ??= new AddTeamCommand(); }
    }

    public string TeamName
    {
        get { return this._teamName ?? string.Empty; }
        set { this.SetField(ref this._teamName, value); }
    }

    public ICommand CompleteAddTeam
    {
        get { return this._completeAddTeam ??= new CompleteAddTeamCommand(this); }
    }

    public ICommand DeleteTeamCommand
    {
        get { return this._deleteTeamCommand ??= new DeleteTeamCommand(); }
    }

    public ICommand EditTeamCommand
    {
        get { return this._editTeamCommand ??= new EditTeamCommand(); }
    }

    private void TeamDeletedEvent_TeamDeleted(object source, TeamDeletedEventArgs teamDeletedEventArgs)
    {
        this.UpdateTeams();
    }

    private void TeamEditedEventTeamEdited(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        this.UpdateTeams();
    }

    private void TeamAddedEvent_TeamAdded(object source, TeamAddedEventArgs teamAddedEventArgs)
    {
        this.UpdateTeams();
    }

    private void TournamentOpenedEvent_TournamentOpened(object                    source,
                                                        TournamentOpenedEventArgs tournamentOpenedEventArgs)
    {
        this.UpdateTeams();
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened += this.TournamentOpenedEvent_TournamentOpened;
        TeamAddedEvent.TeamAdded               += this.TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited            += this.TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted           += this.TeamDeletedEvent_TeamDeleted;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened -= this.TournamentOpenedEvent_TournamentOpened;
        TeamAddedEvent.TeamAdded               -= this.TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited            -= this.TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted           -= this.TeamDeletedEvent_TeamDeleted;
    }

    private void UpdateTeams()
    {
        if (MainViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersViewModel.PlayersContext)
                   .TryGetTeamsWithPlayers(LoginViewModel.CurrentUser.Id,
                                           MainViewModel.SelectedTournament.Id,
                                           out List<Team>? teams);

        if (teams is not null)
        {
            this.TeamsWithPlayers = new ObservableCollection<Team>(teams);
        }
    }

    public void Dispose()
    {
        this.Unsubscribe();
    }
}
