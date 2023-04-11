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
        Subscribe();
    }

    public ICommand ExportGamesListCommand
    {
        get { return _exportGamesListCommand ??= new ExportGamesListCommand(); }
    }

    public ICommand PrintGamesListCommand
    {
        get { return _printGamesListCommand ??= new PrintGamesListCommand(); }
    }

    public ICommand ShowPrevTour
    {
        get { return _showPrevTour ??= new ShowPrevTourCommand(this); }
    }

    public ICommand ShowNextTour
    {
        get { return _showNextTour ??= new ShowNextTourCommand(this); }
    }

    public ICommand StartNewTour
    {
        get { return _startNewTour; }
        private set
        {
            _startNewTour = (StartNewTourCommand?)value;
            OnPropertyChanged();
        }
    }

    public string ToursInfo
    {
        get { return $"Список пар {SelectedTour} тура из {ToursAmount}. Текущий тур: {CurrentTour}"; }
    }

    private int ToursAmount
    {
        get { return _toursAmount; }
        set
        {
            if (SetField(ref _toursAmount, value))
            {
                OnPropertyChanged(nameof(ToursInfo));
            }
        }
    }

    internal int SelectedTour
    {
        get { return _selectedTour; }
        set
        {
            if (SetField(ref _selectedTour, value))
            {
                OnPropertyChanged(nameof(ToursInfo));
                OnPropertyChanged(nameof(GamesForSelectedTour));
            }
        }
    }

    public ObservableCollection<Game> GamesForSelectedTour
    {
        get
        {
            if (_games is null)
            {
                return new ObservableCollection<Game>();
            }

            return new ObservableCollection<Game>(_games.Where(game => game.TourNumber == SelectedTour));
        }
    }

    public int CurrentTour
    {
        get { return _currentTour; }
        set
        {
            if (SetField(ref _currentTour, value))
            {
                OnPropertyChanged(nameof(ToursInfo));
            }
        }
    }

    private void TournamentOpenedEvent_TournamentOpened(object source, TournamentOpenedEventArgs e)
    {
        if (e.OpenedTournament is null)
        {
            return;
        }

        ResetProperties();

        OpenedTournament = e.OpenedTournament;
        ToursAmount      = OpenedTournament.ToursCount;

        UpdateGames();
        if (_games?.Count > 0)
        {
            CurrentTour  = _games.Max(game => game.TourNumber);
            SelectedTour = CurrentTour;
        }

        StartNewTour = new StartNewTourCommand(this);
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        TourAddedEvent.TourAdded               += TourAddedEvent_TourAdded;
        PlayerEditedEvent.PlayerEdited         += PlayerEditedEvent_PlayerEdited;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened -= TournamentOpenedEvent_TournamentOpened;
        TourAddedEvent.TourAdded               -= TourAddedEvent_TourAdded;
        PlayerEditedEvent.PlayerEdited         -= PlayerEditedEvent_PlayerEdited;
    }

    private void PlayerEditedEvent_PlayerEdited(object source, PlayerEditedEventArgs e)
    {
        OnPropertyChanged(nameof(GamesForSelectedTour));
    }

    private void ResetProperties()
    {
        OpenedTournament = null;
        ToursAmount      = 0;
        CurrentTour      = 0;
        SelectedTour     = 0;
        _games           = null;
    }

    private void TourAddedEvent_TourAdded(object sender, TourAddedEventArgs e)
    {
        UpdateGames();
        CurrentTour  = e.TourNumber;
        SelectedTour = CurrentTour;
    }

    private void UpdateGames()
    {
        if (OpenedTournament == null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersViewModel.PlayersContext)
                   .TryGetGames(OpenedTournament.OrganizerId, OpenedTournament.TournamentId,
                                out List<Game>? games);

        _games = games?.OrderByDescending(game => game.PlayerWhite.PointsCount + game.PlayerBlack.PointsCount)
                       .ToList();
        OnPropertyChanged(nameof(GamesForSelectedTour));
    }

    public void Dispose()
    {
        Unsubscribe();
    }
}
