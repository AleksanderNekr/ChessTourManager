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

    private int                         _currentTour;
    private ExportGamesListCommand?     _exportGamesListCommand;
    private ObservableCollection<Game>? _pairs;
    private ObservableCollection<Game>? _pairsForSelectedTour;
    private PrintGamesListCommand?      _printGamesListCommand;
    private int?                        _selectedTour;
    private ShowNextTourCommand?        _showNextTour;
    private ShowPrevTourCommand?        _showPrevTour;
    private StartNewTourCommand?        _startNewTour;
    private Tournament?                 _tournament;

    public PairsGridViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    public string ToursInfo
    {
        get { return $"Выбранный тур: {SelectedTour}, текущий: {CurrentTour}"; }
    }

    public ObservableCollection<Game>? Pairs
    {
        get
        {
            if (_pairs is { })
            {
                return _pairs;
            }

            if (_tournament is null)
            {
                return new ObservableCollection<Game>();
            }

            UpdatePairs();

            return _pairs;
        }
        private set
        {
            SetField(ref _pairs, value);
            SelectedTour = CurrentTour;
        }
    }

    public int SelectedTour
    {
        get
        {
            if (_selectedTour is null)
            {
                SetField(ref _selectedTour, CurrentTour);
            }

            if (_selectedTour is { })
            {
                return (int)_selectedTour;
            }

            return 0;
        }
        set
        {
            SetField(ref _selectedTour, value);
            UpdatePairsForSelectedTour();
            OnPropertyChanged(nameof(ToursInfo));
        }
    }

    public int CurrentTour
    {
        get
        {
            if (Pairs is { } && Pairs.Count != 0)
            {
                UpdateCurrentTour();
            }

            return _currentTour;
        }
        set
        {
            SetField(ref _currentTour, value);
            SelectedTour = _currentTour;
            OnPropertyChanged(nameof(ToursInfo));
        }
    }

    public ObservableCollection<Game>? PairsForSelectedTour
    {
        get
        {
            if (_pairsForSelectedTour is null)
            {
                UpdatePairsForSelectedTour();
            }

            return _pairsForSelectedTour;
        }
        set { SetField(ref _pairsForSelectedTour, value); }
    }

    public Player? DummyPlayer { get; set; }

    public ICommand StartNewTour
    {
        get { return _startNewTour ??= new StartNewTourCommand(this); }
    }

    public ICommand ShowPrevTour
    {
        get { return _showPrevTour ??= new ShowPrevTourCommand(this); }
    }

    public ICommand ShowNextTour
    {
        get { return _showNextTour ??= new ShowNextTourCommand(this); }
    }

    public ICommand ExportGamesListCommand
    {
        get { return _exportGamesListCommand ??= new ExportGamesListCommand(); }
    }

    public ICommand PrintGamesListCommand
    {
        get { return _printGamesListCommand ??= new PrintGamesListCommand(); }
    }

    public string Result { get; set; }

    private void TourAddedEvent_TourAdded(object sender, TourAddedEventArgs tourAddedEventArgs)
    {
        CurrentTour = tourAddedEventArgs.TourNumber;
        UpdatePairs();
        UpdateCurrentTour();
        UpdatePairsForSelectedTour();
    }

    private void UpdateCurrentTour()
    {
        if (Pairs is { Count: 0 })
        {
            SetField(ref _currentTour, 0, nameof(CurrentTour));
        }
        else if (Pairs is { })
        {
            int maxTour = Pairs.Max(p => p.TourNumber);
            SetField(ref _currentTour, maxTour, nameof(CurrentTour));
        }

        SetField(ref _selectedTour, _currentTour, nameof(SelectedTour));
    }

    private void UpdatePairsForSelectedTour()
    {
        if (Pairs is { })
        {
            SetField(ref _pairsForSelectedTour,
                     new ObservableCollection<Game>(Pairs.Where(p => p.TourNumber == SelectedTour)),
                     nameof(PairsForSelectedTour));
        }
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        TourAddedEvent.TourAdded += TourAddedEvent_TourAdded;

        _tournament = e.OpenedTournament;
        UpdatePairs();
        UpdateCurrentTour();
        UpdatePairsForSelectedTour();
    }

    private void UpdatePairs()
    {
        if (_tournament is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PairsContext)
                   .TryGetGames(_tournament.OrganizerId, _tournament.TournamentId,
                                out IEnumerable<Game>? games);

        if (games is null)
        {
            return;
        }

        Pairs = new ObservableCollection<Game>(games);
    }
}
