using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames.Navigation;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public class PairsGridViewModel : ViewModelBase
{
    internal static readonly ChessTourContext PairsContext = PlayersViewModel.PlayersContext;

    internal Tournament? OpenedTournament { get; private set; }

    private ExportGamesListCommand? _exportGamesListCommand;
    private PrintGamesListCommand?  _printGamesListCommand;
    private ShowNextTourCommand?    _showNextTour;
    private ShowPrevTourCommand?    _showPrevTour;
    private StartNewTourCommand?    _startNewTour;
    private List<Game>?             _games;
    private int                     _selectedTour;
    private int                     _toursAmount;
    private int                     _currentTour;

    public PairsGridViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
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

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        OpenedTournament = e.OpenedTournament;
        if (OpenedTournament != null)
        {
            ToursAmount = OpenedTournament.ToursCount;
        }

        UpdateGames();
        if (_games?.Count > 0)
        {
            CurrentTour  = _games.Max(game => game.TourNumber);
            SelectedTour = CurrentTour;
        }

        SetField(ref _startNewTour, new StartNewTourCommand(this), nameof(StartNewTour));
        TourAddedEvent.TourAdded += TourAddedEvent_TourAdded;
    }

    private void TourAddedEvent_TourAdded(object sender, TourAddedEventArgs e)
    {
        UpdateGames();
        CurrentTour  = e.TourNumber;
        SelectedTour = CurrentTour;
    }

    private void UpdateGames()
    {
        IGetQueries.CreateInstance(PairsContext)
                   .TryGetGames(OpenedTournament.OrganizerId, OpenedTournament.TournamentId,
                                out List<Game>? games);
        _games = games;
    }
}
