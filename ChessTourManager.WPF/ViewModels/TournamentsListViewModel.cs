using System.Collections.ObjectModel;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChessTourManager.WPF.ViewModels;

public partial class TournamentsListViewModel : ObservableObject
{
    private static readonly ChessTourContext ChessTourContext = ChessTourContext.CreateInstance();

    [ObservableProperty]
    private bool _isOpened;


    [ObservableProperty]
    private Tournament? _selectedTournament;

    private string? _selectedTournamentName;

    [ObservableProperty]
    private ObservableCollection<Tournament> _tournamentsCollection =
        new(ChessTourContext.Tournaments.Select(tournament => tournament));

    public string SelectedTournamentName
    {
        get
        {
            if (_selectedTournamentName == null)
            {
                _selectedTournamentName = "";
                OnPropertyChanged();
            }

            return _selectedTournamentName;
        }
        private set
        {
            _selectedTournamentName = value;
            OnPropertyChanged();
        }
    }

    [RelayCommand]
    private void OpenTournament(object obj)
    {
        if (obj is Tournament tournament)
        {
            SelectedTournament     = tournament;
            SelectedTournamentName = tournament.TournamentName;
            IsOpened               = false;
            IsOpened               = true;
        }
    }
}
