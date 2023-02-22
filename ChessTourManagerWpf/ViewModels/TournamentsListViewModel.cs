using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ChessTourManagerWpf.Models;
using ChessTourManagerWpf.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChessTourManagerWpf.ViewModels;

public partial class TournamentsListViewModel : ObservableObject
{
    private static readonly ChessTourContext ChessTourContext = new();

    [ObservableProperty]
    private ObservableCollection<Tournament> _tournamentsCollection =
        new(ChessTourContext.Tournaments.Select(tournament => tournament));


    [ObservableProperty]
    private Tournament? _selectedTournament;

    private string? _selectedTournamentName;

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

    [ObservableProperty]
    private bool _isOpened;

    [RelayCommand]
    private void OpenTournament(object obj)
    {
        if (obj is Tournament tournament)
        {
            SelectedTournament     = tournament;
            SelectedTournamentName = tournament.TournamentName;
            IsOpened               = true;
        }
    }
}
