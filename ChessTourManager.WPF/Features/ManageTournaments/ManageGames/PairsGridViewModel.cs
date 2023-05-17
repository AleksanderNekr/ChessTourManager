using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames.Navigation;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public class PairsGridViewModel : ViewModelBase, IDisposable
{
    internal Tournament? OpenedTournament { get; private set; }

    private ExportGamesListCommand? _exportGamesListCommand;
    private PrintGamesListCommand?  _printGamesListCommand;
    private ShowNextTourCommand?    _showNextTour;
    private ShowPrevTourCommand?    _showPrevTour;
    private List<Game>?             _games;
    private int                     _selectedTour;
    private int                     _toursAmount;
    private int                     _currentTour;
    private StartNewTourCommand?    _startNewTour;

    public PairsGridViewModel()
    {
        this.Subscribe();
    }

    public ICommand ExportGamesListCommand
    {
        get { return this._exportGamesListCommand ??= new ExportGamesListCommand(); }
    }

    public ICommand PrintGamesListCommand
    {
        get { return this._printGamesListCommand ??= new PrintGamesListCommand(); }
    }

    public ICommand ShowPrevTour
    {
        get { return this._showPrevTour ??= new ShowPrevTourCommand(this); }
    }

    public ICommand ShowNextTour
    {
        get { return this._showNextTour ??= new ShowNextTourCommand(this); }
    }

    public ICommand StartNewTour
    {
        get { return this._startNewTour; }
        private set
        {
            this._startNewTour = (StartNewTourCommand?)value;
            this.OnPropertyChanged();
        }
    }

    public string ToursInfo
    {
        get { return $"Список пар {this.SelectedTour} тура из {this.ToursAmount}. Текущий тур: {this.CurrentTour}"; }
    }

    private int ToursAmount
    {
        get { return this._toursAmount; }
        set
        {
            if (this.SetField(ref this._toursAmount, value))
            {
                this.OnPropertyChanged(nameof(this.ToursInfo));
            }
        }
    }

    internal int SelectedTour
    {
        get { return this._selectedTour; }
        set
        {
            if (this.SetField(ref this._selectedTour, value))
            {
                this.OnPropertyChanged(nameof(this.ToursInfo));
                this.OnPropertyChanged(nameof(this.GamesForSelectedTour));
            }
        }
    }

    public ObservableCollection<Game> GamesForSelectedTour
    {
        get
        {
            if (this._games is null)
            {
                return new ObservableCollection<Game>();
            }

            return new ObservableCollection<Game>(this._games.Where(game => game.TourNumber == this.SelectedTour));
        }
    }

    public int CurrentTour
    {
        get { return this._currentTour; }
        set
        {
            if (this.SetField(ref this._currentTour, value))
            {
                this.OnPropertyChanged(nameof(this.ToursInfo));
            }
        }
    }

    private void TournamentOpenedEvent_TournamentOpened(object source, TournamentOpenedEventArgs e)
    {
        if (e.OpenedTournament is null)
        {
            return;
        }

        this.ResetProperties();

        this.OpenedTournament = e.OpenedTournament;
        this.ToursAmount      = this.OpenedTournament.ToursCount;

        this.UpdateGames();
        if (this._games?.Count > 0)
        {
            this.CurrentTour  = this._games.Max(game => game.TourNumber);
            this.SelectedTour = this.CurrentTour;
        }

        this.StartNewTour = new StartNewTourCommand(this);
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened += this.TournamentOpenedEvent_TournamentOpened;
        TourAddedEvent.TourAdded               += this.TourAddedEvent_TourAdded;
        PlayerEditedEvent.PlayerEdited         += this.PlayerEditedEvent_PlayerEdited;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened -= this.TournamentOpenedEvent_TournamentOpened;
        TourAddedEvent.TourAdded               -= this.TourAddedEvent_TourAdded;
        PlayerEditedEvent.PlayerEdited         -= this.PlayerEditedEvent_PlayerEdited;
    }

    private void PlayerEditedEvent_PlayerEdited(object source, PlayerEditedEventArgs e)
    {
        this.OnPropertyChanged(nameof(this.GamesForSelectedTour));
    }

    private void ResetProperties()
    {
        this.OpenedTournament = null;
        this.ToursAmount      = 0;
        this.CurrentTour      = 0;
        this.SelectedTour     = 0;
        this._games              = null;
    }

    private void TourAddedEvent_TourAdded(object sender, TourAddedEventArgs e)
    {
        this.UpdateGames();
        this.CurrentTour  = e.TourNumber;
        this.SelectedTour = this.CurrentTour;
    }

    private void UpdateGames()
    {
        if (this.OpenedTournament == null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersViewModel.PlayersContext)
                   .TryGetGames(this.OpenedTournament.OrganizerId, this.OpenedTournament.TournamentId,
                                out List<Game>? games);

        this._games = games?.OrderByDescending(game => game.PlayerWhite.PointsAmount + game.PlayerBlack.PointsAmount)
                            .ToList();
        this.OnPropertyChanged(nameof(this.GamesForSelectedTour));
    }

    public void Dispose()
    {
        this.Unsubscribe();
    }
}
