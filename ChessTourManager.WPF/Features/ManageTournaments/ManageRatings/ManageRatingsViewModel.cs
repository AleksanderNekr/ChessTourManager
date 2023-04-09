using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageRatings;

public class ManageRatingsViewModel : ViewModelBase
{
    private static readonly ChessTourContext RatingsContext = PlayersViewModel.PlayersContext;

    private ObservableCollection<Player>? _playersSorted;
    private ExportRatingListCommand?      _exportRatingListCommand;
    private PrintRatingListCommand?       _printRatingListCommand;
    private string?                       _title;

    public ManageRatingsViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    public ObservableCollection<Player>? PlayersSorted
    {
        get
        {
            if (_playersSorted is null)
            {
                UpdateRating();
            }

            return _playersSorted;
        }
        private set { SetField(ref _playersSorted, value); }
    }

    public string Title
    {
        get { return _title ?? string.Empty; }
        private set { SetField(ref _title, value); }
    }

    public ICommand ExportRatingListCommand
    {
        get { return _exportRatingListCommand ??= new ExportRatingListCommand(); }
    }

    public ICommand PrintRatingListCommand
    {
        get { return _printRatingListCommand ??= new PrintRatingListCommand(); }
    }

    private void TourAddedEvent_TourAdded(object sender, TourAddedEventArgs e)
    {
        Title = $"Рейтинг-лист после {e.TourNumber} тура";
    }

    private void ResultChangedEvent_ResultChanged(object? sender, ResultChangedEventArgs e)
    {
        UpdateRating();
    }

    private void PlayerEditedEvent_PlayerEdited(object source, PlayerEditedEventArgs playerEditedEventArgs)
    {
        UpdateRating();
    }

    private void GroupDeletedEvent_GroupDeleted(object source, GroupDeletedEventArgs groupDeletedEventArgs)
    {
        UpdateRating();
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        UpdateRating();
    }

    private void TeamDeletedEvent_TeamDeleted(object source, TeamDeletedEventArgs teamDeletedEventArgs)
    {
        UpdateRating();
    }

    private void TeamEditedEventTeamEdited(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        UpdateRating();
    }

    private void PlayerDeletedEvent_PlayerDeleted(object source, PlayerDeletedEventArgs playerDeletedEventArgs)
    {
        UpdateRating();
    }

    private void PlayerAddedEvent_PlayerAdded(object source, PlayerAddedEventArgs playerAddedEventArgs)
    {
        UpdateRating();
    }

    private void TournamentEditedEvent_TournamentEdited(object source, TournamentEditedEventArgs tournamentEditedEventArgs)
    {
        UpdateRating();
    }

    private void TournamentOpenedEvent_TournamentOpened(object source, TournamentOpenedEventArgs tournamentOpenedEventArgs)
    {
        UpdateRating();

        UpdateTitle();

        Subscribe();
    }

    private void UpdateTitle()
    {
        if (LoginViewModel.CurrentUser is null)
        {
            return;
        }

        if (MainViewModel.SelectedTournament is null)
        {
            return;
        }

        IGetQueries.CreateInstance(RatingsContext)
                   .TryGetGames(LoginViewModel.CurrentUser.UserId,
                                MainViewModel.SelectedTournament.TournamentId,
                                out List<Game>? games);

        if (games is null)
        {
            return;
        }

        UpdateTitle(games);
    }

    private void UpdateTitle(IEnumerable<Game> games)
    {
        IEnumerable<Game> gamesEnum = games as Game[] ?? games.ToArray();
        if (!gamesEnum.Any())
        {
            Title = "Рейтинг-лист";
        }
        else
        {
            // Get the number of the last tour.
            int lastTourNumber = gamesEnum.Max(g => g.TourNumber);
            Title = $"Рейтинг-лист после {lastTourNumber} тура";
        }
    }

    private void Subscribe()
    {
        TournamentEditedEvent.TournamentEdited += TournamentEditedEvent_TournamentEdited;

        PlayerAddedEvent.PlayerAdded     += PlayerAddedEvent_PlayerAdded;
        PlayerEditedEvent.PlayerEdited   += PlayerEditedEvent_PlayerEdited;
        PlayerDeletedEvent.PlayerDeleted += PlayerDeletedEvent_PlayerDeleted;

        TeamChangedEvent.TeamEdited   += TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted += TeamDeletedEvent_TeamDeleted;

        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted += GroupDeletedEvent_GroupDeleted;

        ResultChangedEvent.ResultChanged += ResultChangedEvent_ResultChanged;

        TourAddedEvent.TourAdded += TourAddedEvent_TourAdded;
    }

    private void UpdateRating()
    {
        PlayersSorted = new ObservableCollection<Player>(GetRating() ?? Enumerable.Empty<Player>());
    }

    private static IOrderedEnumerable<Player>? GetRating()
    {
        if (MainViewModel.SelectedTournament is null)
        {
            return null;
        }

        if (LoginViewModel.CurrentUser is null)
        {
            return null;
        }

        IGetQueries.CreateInstance(RatingsContext)
                   .TryGetPlayersWithTeamsAndGroups(LoginViewModel.CurrentUser.UserId,
                                                    MainViewModel.SelectedTournament.TournamentId,
                                                    out List<Player>? players);

        return GetSortedPlayers(players);
    }

    private static IOrderedEnumerable<Player>? GetSortedPlayers(IEnumerable<Player>? players)
    {
        return players?.OrderByDescending(p => p.PointsCount)
                       .ThenByDescending(p => p.RatioSum1)
                       .ThenByDescending(p => p.RatioSum2);
    }
}
