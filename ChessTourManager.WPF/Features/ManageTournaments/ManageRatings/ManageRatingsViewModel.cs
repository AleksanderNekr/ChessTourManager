using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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

public class ManageRatingsViewModel : ViewModelBase, IDisposable
{
    private ObservableCollection<Player>? _playersSorted;
    private ExportRatingListCommand?      _exportRatingListCommand;
    private PrintRatingListCommand?       _printRatingListCommand;
    private string?                       _title;

    public ManageRatingsViewModel()
    {
        this.Subscribe();
    }

    public ObservableCollection<Player>? PlayersSorted
    {
        get
        {
            if (this._playersSorted is null)
            {
                this.UpdateRating();
            }

            return this._playersSorted;
        }
        private set { this.SetField(ref this._playersSorted, value); }
    }

    public string Title
    {
        get { return this._title ?? string.Empty; }
        private set { this.SetField(ref this._title, value); }
    }

    public ICommand ExportRatingListCommand
    {
        get { return this._exportRatingListCommand ??= new ExportRatingListCommand(); }
    }

    public ICommand PrintRatingListCommand
    {
        get { return this._printRatingListCommand ??= new PrintRatingListCommand(); }
    }

    private void TourAddedEvent_TourAdded(object sender, TourAddedEventArgs e)
    {
        this.Title = $"Рейтинг-лист после {e.TourNumber} тура";
    }

    private void ResultChangedEvent_ResultChanged(object? sender, ResultChangedEventArgs e)
    {
        this.UpdateRating();
    }

    private void PlayerEditedEvent_PlayerEdited(object source, PlayerEditedEventArgs playerEditedEventArgs)
    {
        this.UpdateRating();
    }

    private void GroupDeletedEvent_GroupDeleted(object source, GroupDeletedEventArgs groupDeletedEventArgs)
    {
        this.UpdateRating();
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        this.UpdateRating();
    }

    private void TeamDeletedEvent_TeamDeleted(object source, TeamDeletedEventArgs teamDeletedEventArgs)
    {
        this.UpdateRating();
    }

    private void TeamEditedEventTeamEdited(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        this.UpdateRating();
    }

    private void PlayerDeletedEvent_PlayerDeleted(object source, PlayerDeletedEventArgs playerDeletedEventArgs)
    {
        this.UpdateRating();
    }

    private void PlayerAddedEvent_PlayerAdded(object source, PlayerAddedEventArgs playerAddedEventArgs)
    {
        this.UpdateRating();
    }

    private void TournamentEditedEvent_TournamentEdited(object source, TournamentEditedEventArgs tournamentEditedEventArgs)
    {
        this.UpdateRating();
    }

    private void TournamentOpenedEvent_TournamentOpened(object source, TournamentOpenedEventArgs tournamentOpenedEventArgs)
    {
        this.UpdateRating();
        this.UpdateTitle();
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

        IGetQueries.CreateInstance(PlayersViewModel.PlayersContext)
                   .TryGetGames(LoginViewModel.CurrentUser.Id,
                                MainViewModel.SelectedTournament.Id,
                                out List<Game>? games);

        if (games is null)
        {
            return;
        }

        this.UpdateTitle(games);
    }

    private void UpdateTitle(IEnumerable<Game> games)
    {
        IEnumerable<Game> gamesEnum = games as Game[] ?? games.ToArray();
        if (!gamesEnum.Any())
        {
            this.Title = "Рейтинг-лист";
        }
        else
        {
            // Get the number of the last tour.
            int lastTourNumber = gamesEnum.Max(g => g.TourNumber);
            this.Title = $"Рейтинг-лист после {lastTourNumber} тура";
        }
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened += this.TournamentOpenedEvent_TournamentOpened;
        TournamentEditedEvent.TournamentEdited += this.TournamentEditedEvent_TournamentEdited;

        PlayerAddedEvent.PlayerAdded     += this.PlayerAddedEvent_PlayerAdded;
        PlayerEditedEvent.PlayerEdited   += this.PlayerEditedEvent_PlayerEdited;
        PlayerDeletedEvent.PlayerDeleted += this.PlayerDeletedEvent_PlayerDeleted;

        TeamChangedEvent.TeamEdited  += this.TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted += this.TeamDeletedEvent_TeamDeleted;

        GroupChangedEvent.GroupChanged += this.GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted += this.GroupDeletedEvent_GroupDeleted;

        ResultChangedEvent.ResultChanged += this.ResultChangedEvent_ResultChanged;

        TourAddedEvent.TourAdded += this.TourAddedEvent_TourAdded;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened -= this.TournamentOpenedEvent_TournamentOpened;
        TournamentEditedEvent.TournamentEdited -= this.TournamentEditedEvent_TournamentEdited;

        PlayerAddedEvent.PlayerAdded     -= this.PlayerAddedEvent_PlayerAdded;
        PlayerEditedEvent.PlayerEdited   -= this.PlayerEditedEvent_PlayerEdited;
        PlayerDeletedEvent.PlayerDeleted -= this.PlayerDeletedEvent_PlayerDeleted;

        TeamChangedEvent.TeamEdited  -= this.TeamEditedEventTeamEdited;
        TeamDeletedEvent.TeamDeleted -= this.TeamDeletedEvent_TeamDeleted;

        GroupChangedEvent.GroupChanged -= this.GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted -= this.GroupDeletedEvent_GroupDeleted;

        ResultChangedEvent.ResultChanged -= this.ResultChangedEvent_ResultChanged;

        TourAddedEvent.TourAdded -= this.TourAddedEvent_TourAdded;
    }

    private void UpdateRating()
    {
        this.PlayersSorted = new ObservableCollection<Player>(GetRating() ?? Enumerable.Empty<Player>());
    }

    private static IOrderedEnumerable<Player>? GetRating()
    {
        if (MainViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return null;
        }

        IGetQueries.CreateInstance(PlayersViewModel.PlayersContext)
                   .TryGetPlayersWithTeamsAndGroups(LoginViewModel.CurrentUser.Id,
                                                    MainViewModel.SelectedTournament.Id,
                                                    out List<Player>? players);

        return GetSortedPlayers(players);
    }

    private static IOrderedEnumerable<Player>? GetSortedPlayers(IEnumerable<Player>? players)
    {
        return players?.OrderByDescending(p => p.PointsAmount)
                       .ThenByDescending(p => p.RatioSum1)
                       .ThenByDescending(p => p.RatioSum2);
    }

    public void Dispose()
    {
        this.Unsubscribe();
    }
}
