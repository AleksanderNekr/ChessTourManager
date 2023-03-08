﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.ChangeTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;

public class ManageTeamsViewModel : ViewModelBase
{
    private AddTeamCommand? _addTeamCommand;

    private ObservableCollection<Team> _teamsWithPlayers;
    private string                     _teamName;

    internal static ChessTourContext TeamsContext = PlayersViewModel.PlayersContext;

    public ManageTeamsViewModel()
    {
        CompleteAddTeam                        =  new CompleteAddTeamCommand(this);
        DeleteTeamCommand                      =  new DeleteTeamCommand();
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        TeamAddedEvent.TeamAdded               += TeamAddedEvent_TeamAdded;
        TeamChangedEvent.TeamChanged           += TeamChangedEvent_TeamChanged;
        TeamDeletedEvent.TeamDeleted           += TeamDeletedEvent_TeamDeleted;
    }

    private void TeamDeletedEvent_TeamDeleted(TeamDeletedEventArgs e)
    {
        UpdateTeams();
    }

    private void TeamChangedEvent_TeamChanged(TeamChangedEventArgs e)
    {
        UpdateTeams();
    }

    private void TeamAddedEvent_TeamAdded(TeamAddedEventArgs e)
    {
        UpdateTeams();
    }

    public ObservableCollection<Team> TeamsWithPlayers
    {
        get { return _teamsWithPlayers; }
        private set { SetField(ref _teamsWithPlayers, value); }
    }

    public ICommand AddTeamCommand
    {
        get { return _addTeamCommand ??= new AddTeamCommand(this); }
    }

    public string TeamName
    {
        get { return _teamName; }
        set { SetField(ref _teamName, value); }
    }

    public ICommand CompleteAddTeam   { get; }
    public ICommand DeleteTeamCommand { get; }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        UpdateTeams();
    }

    internal void UpdateTeams()
    {
        IGetQueries.CreateInstance(TeamsContext)
                   .TryGetTeamsWithPlayers(LoginViewModel.CurrentUser!.UserId,
                                           TournamentsListViewModel.SelectedTournament!.TournamentId,
                                           out IQueryable<Team>? teams);
        if (teams is not null)
        {
            TeamsWithPlayers = new ObservableCollection<Team>(teams);
        }
    }
}
