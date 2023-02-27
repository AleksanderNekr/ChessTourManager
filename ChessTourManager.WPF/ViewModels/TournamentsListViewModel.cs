using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
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
        GetResult result = IGetQueries.CreateInstance()
                                      .TryGetTournaments(2, out IEnumerable<Tournament>? tournamentsCollection);

        switch (result)
        {
            case GetResult.Success:
                TournamentsCollection = new ObservableCollection<Tournament>(tournamentsCollection);
                break;
            case GetResult.UserNotFound:
                MessageBox.Show("Пользователь не найден!", "Ошибка получения списка турниров",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
