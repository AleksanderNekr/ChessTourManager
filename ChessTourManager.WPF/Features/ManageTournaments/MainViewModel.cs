using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;
using ChessTourManager.WPF.Features.ManageTournaments.DeleteTournament;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments;

public class MainViewModel : ViewModelBase
{
    internal static readonly ChessTourContext MainContext = new();

    private bool                              _isOpened;
    private DeleteTournamentCommand?          _deleteTournamentCommand;
    private OpenTournamentCommand?            _openTournamentCommand;
    private StartEditTournamentCommand?       _startEditTournamentCommand;
    private ObservableCollection<Tournament>? _tournamentsCollection;

    public MainViewModel()
    {
        TournamentOpenedEvent.TournamentOpened   += TournamentOpenedEvent_TournamentOpened;
        TournamentCreatedEvent.TournamentCreated += TournamentCreatedEvent_TournamentCreated;
        TournamentEditedEvent.TournamentEdited   += TournamentEditedEvent_TournamentEdited;
        TournamentDeletedEvent.TournamentDeleted += TournamentDeletedEvent_TournamentDeleted;

        UpdateTournamentsList();
    }

    public bool IsOpened
    {
        get { return _isOpened; }
        set
        {
            _isOpened = value;
            OnPropertyChanged();
        }
    }

    public static Tournament? SelectedTournament { get; set; }

    public Tournament? SelectedTournamentObservable
    {
        get { return SelectedTournament; }
    }

    public ICommand OpenTournamentCommand
    {
        get { return _openTournamentCommand ??= new OpenTournamentCommand(this); }
    }

    public ObservableCollection<Tournament>? TournamentsCollection
    {
        get { return _tournamentsCollection; }
        set { SetField(ref _tournamentsCollection, value); }
    }

    public ICommand DeleteTournamentCommand
    {
        get { return _deleteTournamentCommand ??= new DeleteTournamentCommand(this); }
    }

    public ICommand StartEditTournamentCommand
    {
        get { return _startEditTournamentCommand ??= new StartEditTournamentCommand(); }
    }

    private void TournamentDeletedEvent_TournamentDeleted(object source, DeleteTournamentEventArgs e)
    {
        UpdateTournamentsList();
        if (SelectedTournament?.Equals(e.DeletedTournament) ?? false)
        {
            IsOpened = false;
        }
    }

    private void TournamentEditedEvent_TournamentEdited(object source, TournamentEditedEventArgs e)
    {
        UpdateTournamentsList();
    }

    private void TournamentCreatedEvent_TournamentCreated(object source, TournamentCreatedEventArgs e)
    {
        UpdateTournamentsList();
    }

    internal void UpdateTournamentsList()
    {
        if (LoginViewModel.CurrentUser == null)
        {
            return;
        }

        GetResult result = IGetQueries.CreateInstance(MainContext)
                                      .TryGetTournaments(LoginViewModel.CurrentUser.UserId,
                                                         out List<Tournament>? tournamentsCollection);

        if (result == GetResult.Success)
        {
            TournamentsCollection = new ObservableCollection<Tournament>(tournamentsCollection);
        }
        else if (result == GetResult.UserNotFound)
        {
            MessageBox.Show("Пользователь не найден!", "Ошибка получения списка турниров",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void TournamentOpenedEvent_TournamentOpened(object source, TournamentOpenedEventArgs e)
    {
        OnPropertyChanged(nameof(SelectedTournamentObservable));
    }
}
