using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Get;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public class PairsGridViewModel : ViewModelBase
{
    private Tournament? _tournament;

    public PairsGridViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        _tournament = e.OpenedTournament;
        UpdatePairs();
    }

    private void UpdatePairs()
    {
        IGetQueries.CreateInstance(PairsContext)
                   .TryGetGames(_tournament!.OrganizerId,
                                _tournament.TournamentId,
                                out IQueryable<Game>? games);

        if (games is null)
        {
            return;
        }

        Pairs                = new ObservableCollection<Game>(games);
        PairsForSelectedTour = new ObservableCollection<Game>(Pairs.Where(p => p.TourNumber == CurrentTour));
    }

    internal static readonly ChessTourContext PairsContext = new();

    private int                         _currentTour;
    private ObservableCollection<Game>? _pairs;
    private int?                        _selectedTour;
    private ObservableCollection<Game>? _pairsForSelectedTour;
    public  string                      ToursInfo => $"Выбранный тур: {SelectedTour}, текущий: {CurrentTour}";

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
            OnPropertyChanged(nameof(ToursInfo));
            OnPropertyChanged(nameof(PairsForSelectedTour));
        }
    }

    public int CurrentTour
    {
        get
        {
            if (Pairs.Count == 0)
            {
                return 0;
            }

            Game currentPair = Pairs.Last();
            SetField(ref _currentTour, currentPair.TourNumber);

            return _currentTour;
        }
        set
        {
            SetField(ref _currentTour, value);
            SelectedTour = _currentTour;
        }
    }

    public ObservableCollection<Game> PairsForSelectedTour
    {
        get
        {
            if (_pairsForSelectedTour is null)
            {
                SetField(ref _pairsForSelectedTour,
                         new ObservableCollection<Game>(Pairs.Where(p => p.TourNumber == CurrentTour)));
            }

            return _pairsForSelectedTour!;
        }
        set { SetField(ref _pairsForSelectedTour, value); }
    }
}
