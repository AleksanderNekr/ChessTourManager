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

public class ManageTeamsViewModel : ViewModelBase
{
    internal static readonly ChessTourContext TeamsContext = PlayersViewModel.PlayersContext;
    private                  AddTeamCommand?  _addTeamCommand;
    private                  string?          _teamName;

    private ObservableCollection<Team> _teamsWithPlayers;

    public ManageTeamsViewModel()
    {
        CompleteAddTeam                        =  new CompleteAddTeamCommand(this);
        DeleteTeamCommand                      =  new DeleteTeamCommand();
        EditTeamCommand                        =  new EditTeamCommand();
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        TeamAddedEvent.TeamAdded               += TeamAddedEvent_TeamAdded;
        TeamEditedEvent.TeamEdited           += TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted           += TeamDeletedEvent_TeamDeleted;
    }

    public ObservableCollection<Team> TeamsWithPlayers
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
        get { return _teamName ?? "Введите название команды"; }
        set { SetField(ref _teamName, value); }
    }

    public ICommand CompleteAddTeam   { get; }
    public ICommand DeleteTeamCommand { get; }
    public ICommand EditTeamCommand   { get; }

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

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        UpdateTeams();
    }

    private void UpdateTeams()
    {
        IGetQueries.CreateInstance(TeamsContext)
                   .TryGetTeamsWithPlayers(LoginViewModel.CurrentUser!.UserId,
                                           TournamentsListViewModel.SelectedTournament!.TournamentId,
                                           out IEnumerable<Team>? teams);
        if (teams is not null)
        {
            TeamsWithPlayers = new ObservableCollection<Team>(teams);
        }
    }
}
