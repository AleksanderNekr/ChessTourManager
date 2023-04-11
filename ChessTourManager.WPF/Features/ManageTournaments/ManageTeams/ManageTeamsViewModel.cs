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
        Subscribe();
    }

    public ObservableCollection<Team>? TeamsWithPlayers
    {
        get { return _teamsWithPlayers; }
        private set { SetField(ref _teamsWithPlayers, value); }
    }

    public ICommand AddTeamCommand
    {
        get { return _addTeamCommand ??= new AddTeamCommand(); }
    }

    public string TeamName
    {
        get { return _teamName ?? string.Empty; }
        set { SetField(ref _teamName, value); }
    }

    public ICommand CompleteAddTeam
    {
        get { return _completeAddTeam ??= new CompleteAddTeamCommand(this); }
    }

    public ICommand DeleteTeamCommand
    {
        get { return _deleteTeamCommand ??= new DeleteTeamCommand(); }
    }

    public ICommand EditTeamCommand
    {
        get { return _editTeamCommand ??= new EditTeamCommand(); }
    }

    private void TeamDeletedEvent_TeamDeleted(object source, TeamDeletedEventArgs teamDeletedEventArgs)
    {
        UpdateTeams();
    }

    private void TeamEditedEventTeamEdited(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        UpdateTeams();
    }

    private void TeamAddedEvent_TeamAdded(object source, TeamAddedEventArgs teamAddedEventArgs)
    {
        UpdateTeams();
    }

    private void TournamentOpenedEvent_TournamentOpened(object                    source,
                                                        TournamentOpenedEventArgs tournamentOpenedEventArgs)
    {
        UpdateTeams();
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        TeamAddedEvent.TeamAdded               += TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited            += TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted           += TeamDeletedEvent_TeamDeleted;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened -= TournamentOpenedEvent_TournamentOpened;
        TeamAddedEvent.TeamAdded               -= TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamEdited            -= TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted           -= TeamDeletedEvent_TeamDeleted;
    }

    private void UpdateTeams()
    {
        if (MainViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersViewModel.PlayersContext)
                   .TryGetTeamsWithPlayers(LoginViewModel.CurrentUser.UserId,
                                           MainViewModel.SelectedTournament.TournamentId,
                                           out List<Team>? teams);

        if (teams is { })
        {
            TeamsWithPlayers = new ObservableCollection<Team>(teams);
        }
    }

    public void Dispose()
    {
        Unsubscribe();
    }
}
