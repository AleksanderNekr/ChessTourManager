using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries;
using ChessTourManager.WPF.Commands;

namespace ChessTourManager.WPF.ViewModels;

public class TournamentsListViewModel : ViewModelBase
{
    private bool _isOpened;

    private OpenTournamentCommand? _openTournamentCommand;

    private Tournament? _selectedTournament;

    public TournamentsListViewModel()
    {
        bool isSuccess = IGetQueries.CreateInstance()
                                    .TryGetTournaments(2, out IEnumerable<Tournament>? tournamentsCollection);

        if (isSuccess)
        {
            TournamentsCollection = new ObservableCollection<Tournament>(tournamentsCollection);
        }
    }

    public bool IsOpened
    {
        get => _isOpened;
        set
        {
            SetField(ref _isOpened, value);
            OnPropertyChanged();
        }
    }

    public Tournament? SelectedTournament
    {
        get => _selectedTournament;
        set => SetField(ref _selectedTournament, value);
    }

    public ICommand OpenTournamentCommand => _openTournamentCommand ??= new OpenTournamentCommand(this);

    public ObservableCollection<Tournament>? TournamentsCollection { get; }
}
