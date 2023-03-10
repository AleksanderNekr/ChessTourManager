using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public class PairsGridViewModel : ViewModelBase
{
    internal static readonly ChessTourContext PairsContext = new();

    private int                         _currentTour;
    private ObservableCollection<Game>? _pairs;
    private ObservableCollection<Game>? _pairsForSelectedTour;
    private int?                        _selectedTour;
    private Tournament?                 _tournament;
    private StartNewTourCommand?        _startNewTour;
    private ShowPrevTourCommand?        _showPrevTour;
    private ShowNextTourCommand?        _showNextTour;

    public PairsGridViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        TourAddedEvent.TourAdded               += TourAddedEvent_TourAdded;
    }

    private void TourAddedEvent_TourAdded(object sender, TourAddedEventArgs tourAddedEventArgs)
    {
        CurrentTour = tourAddedEventArgs.TourNumber;
        UpdatePairs();
        UpdateCurrentTour();
        UpdatePairsForSelectedTour();
    }

    public string ToursInfo
    {
        get { return $"Выбранный тур: {SelectedTour}, текущий: {CurrentTour}"; }
    }

    public ObservableCollection<Game> Pairs
    {
        get
        {
            if (_pairs is not null)
            {
                return _pairs;
            }

            if (_tournament is null)
            {
                return new ObservableCollection<Game>();
            }

            UpdatePairs();

            return _pairs!;
        }
        set
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

            return (int)_selectedTour!;
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
            if (Pairs.Count != 0)
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

    private void UpdateCurrentTour()
    {
        int maxTour = Pairs.Max(p => p.TourNumber);
        SetField(ref _currentTour, maxTour);
    }

    public ObservableCollection<Game> PairsForSelectedTour
    {
        get
        {
            if (_pairsForSelectedTour is null)
            {
                UpdatePairsForSelectedTour();
            }

            return _pairsForSelectedTour!;
        }
        set { SetField(ref _pairsForSelectedTour, value); }
    }

    private void UpdatePairsForSelectedTour()
    {
        SetField(ref _pairsForSelectedTour,
                 new ObservableCollection<Game>(Pairs.Where(p => p.TourNumber == SelectedTour)),
                 nameof(PairsForSelectedTour));
    }

    public ICommand StartNewTour => _startNewTour ??= new StartNewTourCommand(this);
    public ICommand ShowPrevTour => _showPrevTour ??= new ShowPrevTourCommand(this);
    public ICommand ShowNextTour => _showNextTour ??= new ShowNextTourCommand(this);

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        _tournament = e.OpenedTournament;
        UpdatePairs();
        UpdateCurrentTour();
        UpdatePairsForSelectedTour();
    }

    private void UpdatePairs()
    {
        IGetQueries.CreateInstance(PairsContext)
                   .TryGetGames(_tournament!.OrganizerId, _tournament.TournamentId, out IEnumerable<Game>? games);

        if (games is null)
        {
            return;
        }

        Pairs = new ObservableCollection<Game>(games);
    }
}
