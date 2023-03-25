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

public class TreeViewModel : ViewModelBase
{
    internal static readonly ChessTourContext TreeContext = TournamentsListViewModel.TournamentsListContext;
    private                  Player?          _selectedPlayer;
    private                  Team?            _selectedTeam;
    private                  Tournament?      _selectedTournament;

    private ObservableCollection<Tournament>? _tournaments;

    public TreeViewModel()
    {
        TournamentEditedEvent.TournamentEdited   += TournamentEditedEvent_TournamentEdited;
        TournamentCreatedEvent.TournamentCreated += TournamentCreatedEvent_TournamentCreated;
        TournamentDeletedEvent.TournamentDeleted += TournamentDeletedEvent_TournamentDeleted;

        TeamAddedEvent.TeamAdded     += TeamAddedEvent_TeamAdded;
        TeamEditedEvent.TeamEdited   += TeamChangedEvent_TeamChanged;
        TeamDeletedEvent.TeamDeleted += TeamChangedEvent_TeamDeleted;

        PlayerAddedEvent.PlayerAdded     += PlayerChangedEvent_PlayerAdded;
        PlayerDeletedEvent.PlayerDeleted += PlayerChangedEvent_PlayerDeleted;
        PlayerEditedEvent.PlayerEdited   += PlayerChangedEvent_PlayerEdited;
    }

    public ObservableCollection<Tournament> TournamentsRoot
    {
        get
        {
            if (_tournaments is not null || LoginViewModel.CurrentUser == null)
            {
                return _tournaments!;
            }

            UpdateTournaments();

            return _tournaments!;
        }
        set { SetField(ref _tournaments, value); }
    }

    public Tournament SelectedTournament
    {
        get { return _selectedTournament ??= new Tournament(); }
        set { SetField(ref _selectedTournament, value); }
    }

    public Team? SelectedTeam
    {
        get { return _selectedTeam ??= new Team(); }
        set { SetField(ref _selectedTeam, value); }
    }

    public Player? SelectedPlayer
    {
        get { return _selectedPlayer ??= new Player(); }
        set { SetField(ref _selectedPlayer, value); }
    }

    private void PlayerChangedEvent_PlayerEdited(PlayerEditedEventArgs e)
    {
        UpdateTournaments();
    }

    private void PlayerChangedEvent_PlayerDeleted(PlayerDeletedEventArgs e)
    {
        UpdateTournaments();
    }

    private void PlayerChangedEvent_PlayerAdded(PlayerAddedEventArgs e)
    {
        UpdateTournaments();
    }

    private void TeamChangedEvent_TeamDeleted(TeamDeletedEventArgs e)
    {
        UpdateTournaments();
    }

    private void TeamChangedEvent_TeamChanged(TeamChangedEventArgs e)
    {
        UpdateTournaments();
    }

    private void TournamentDeletedEvent_TournamentDeleted(DeleteTournamentEventArgs e)
    {
        UpdateTournaments();
    }

    private void TournamentCreatedEvent_TournamentCreated(TournamentCreatedEventArgs e)
    {
        UpdateTournaments();
    }

    private void TeamAddedEvent_TeamAdded(TeamAddedEventArgs e)
    {
        UpdateTournaments();
    }

    private void TournamentEditedEvent_TournamentEdited(TournamentEditedEventArgs e)
    {
        UpdateTournaments();
    }

    private void UpdateTournaments()
    {
        IGetQueries.CreateInstance(TreeContext)
                   .TryGetTournamentsWithTeamsAndPlayers(LoginViewModel.CurrentUser.UserId,
                                                         out IEnumerable<Tournament>? tournaments);


        if (tournaments is not null)
        {
            SetField(ref _tournaments, new ObservableCollection<Tournament>(tournaments), nameof(TournamentsRoot));
        }
    }
}
