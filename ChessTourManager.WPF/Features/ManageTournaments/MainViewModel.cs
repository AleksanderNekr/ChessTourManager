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

public class MainViewModel : ViewModelBase, IDisposable
{
    internal static readonly ChessTourContext MainContext = new();

    private bool                              _isOpened;
    private DeleteTournamentCommand?          _deleteTournamentCommand;
    private OpenTournamentCommand?            _openTournamentCommand;
    private StartEditTournamentCommand?       _startEditTournamentCommand;
    private ObservableCollection<Tournament>? _tournamentsCollection;

    public MainViewModel()
    {
        this.Subscribe();

        this.UpdateTournamentsList();
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened   += this.TournamentOpenedEvent_TournamentOpened;
        TournamentCreatedEvent.TournamentCreated += this.TournamentCreatedEvent_TournamentCreated;
        TournamentEditedEvent.TournamentEdited   += this.TournamentEditedEvent_TournamentEdited;
        TournamentDeletedEvent.TournamentDeleted += this.TournamentDeletedEvent_TournamentDeleted;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened   -= this.TournamentOpenedEvent_TournamentOpened;
        TournamentCreatedEvent.TournamentCreated -= this.TournamentCreatedEvent_TournamentCreated;
        TournamentEditedEvent.TournamentEdited   -= this.TournamentEditedEvent_TournamentEdited;
        TournamentDeletedEvent.TournamentDeleted -= this.TournamentDeletedEvent_TournamentDeleted;
    }

    public bool IsOpened
    {
        get { return this._isOpened; }
        set
        {
            this._isOpened = value;
            this.OnPropertyChanged();
        }
    }

    public static Tournament? SelectedTournament { get; set; }

    public Tournament? SelectedTournamentObservable
    {
        get { return SelectedTournament; }
    }

    public ICommand OpenTournamentCommand
    {
        get { return this._openTournamentCommand ??= new OpenTournamentCommand(this); }
    }

    public ObservableCollection<Tournament>? TournamentsCollection
    {
        get { return this._tournamentsCollection; }
        set { this.SetField(ref this._tournamentsCollection, value); }
    }

    public ICommand DeleteTournamentCommand
    {
        get { return this._deleteTournamentCommand ??= new DeleteTournamentCommand(this); }
    }

    public ICommand StartEditTournamentCommand
    {
        get { return this._startEditTournamentCommand ??= new StartEditTournamentCommand(); }
    }

    private void TournamentDeletedEvent_TournamentDeleted(object source, DeleteTournamentEventArgs e)
    {
        this.UpdateTournamentsList();
        if (SelectedTournament?.Equals(e.DeletedTournament) ?? false)
        {
            this.IsOpened = false;
        }
    }

    private void TournamentEditedEvent_TournamentEdited(object source, TournamentEditedEventArgs e)
    {
        this.UpdateTournamentsList();
    }

    private void TournamentCreatedEvent_TournamentCreated(object source, TournamentCreatedEventArgs e)
    {
        this.UpdateTournamentsList();
    }

    internal void UpdateTournamentsList()
    {
        if (LoginViewModel.CurrentUser == null)
        {
            return;
        }

        GetResult result = IGetQueries.CreateInstance(MainContext)
                                      .TryGetTournaments(LoginViewModel.CurrentUser.Id,
                                                         out List<Tournament>? tournamentsCollection);

        if (result == GetResult.Success)
        {
            this.TournamentsCollection = new ObservableCollection<Tournament>(tournamentsCollection);
        }
        else if (result == GetResult.UserNotFound)
        {
            MessageBox.Show("Пользователь не найден!", "Ошибка получения списка турниров",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void TournamentOpenedEvent_TournamentOpened(object source, TournamentOpenedEventArgs e)
    {
        this.OnPropertyChanged(nameof(this.SelectedTournamentObservable));
    }

    public void Dispose()
    {
        SelectedTournament = null;
        this.Unsubscribe();
    }
}
