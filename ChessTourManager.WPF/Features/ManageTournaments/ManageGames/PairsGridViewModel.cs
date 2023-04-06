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
    internal static readonly ChessTourContext        PairsContext = PlayersViewModel.PlayersContext;
    private                  ExportGamesListCommand? _exportGamesListCommand;
    private                  PrintGamesListCommand?  _printGamesListCommand;
    private                  ShowNextTourCommand?    _showNextTour;
    private                  ShowPrevTourCommand?    _showPrevTour;
    private                  int                     _selectedTour;

    private int                  _toursAmount;
    private StartNewTourCommand? _startNewTour;
    private int                  _currentTour;

    public PairsGridViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    internal List<Game>? Games { get; private set; }

    public string ToursInfo
    {
        get { return $"Список пар {SelectedTour} тура из {ToursAmount}. Текущий тур: {CurrentTour}"; }
    }

    internal int ToursAmount
    {
        get { return _toursAmount; }
        private set
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
            return Games is null
                       ? new ObservableCollection<Game>()
                       : new ObservableCollection<Game>(Games.Where(game => game.TourNumber == SelectedTour));
        }
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

    public int CurrentTour
    {
        get { return _currentTour; }
        set
        {
            if (SetField(ref _currentTour, value))
            {
                UpdateGames();
                SelectedTour = _currentTour;
            }
        }
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        OpenedTournament = e.OpenedTournament;
        UpdateGames();
        ToursAmount = e.OpenedTournament.ToursCount;
        SelectedTour = Games?.Count > 0
                           ? Games.Max(game => game.TourNumber)
                           : 0;
        CurrentTour = SelectedTour;
        SetField(ref _startNewTour, new StartNewTourCommand(this), nameof(StartNewTour));
    }

    internal Tournament OpenedTournament { get; private set; }

    private void UpdateGames()
    {
        IGetQueries.CreateInstance(PairsContext)
                   .TryGetGames(TournamentsListViewModel.SelectedTournament.OrganizerId,
                                TournamentsListViewModel.SelectedTournament.TournamentId,
                                out List<Game>? games);
        Games = games;
    }
}
