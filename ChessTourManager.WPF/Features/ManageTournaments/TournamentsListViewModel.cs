using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WPF.Features.ManageTournaments;

public class TournamentsListViewModel : ViewModelBase
{
    private static readonly ChessTourContext TournamentsListContext = new();
    private                 bool             _isOpened;

    private OpenTournamentCommand?            _openTournamentCommand;
    private ObservableCollection<Tournament>? _tournamentsCollection;

    public TournamentsListViewModel()
    {
        TournamentOpenedEvent.TournamentOpened   += TournamentOpenedEvent_TournamentOpened;
        TournamentCreatedEvent.TournamentCreated += TournamentCreatedEvent_TournamentCreated;

        UpdateTournamentsList();
    }

    private void TournamentCreatedEvent_TournamentCreated(TournamentCreatedEventArgs e)
    {
        UpdateTournamentsList();
    }

    private void UpdateTournamentsList()
    {
        GetResult result = IGetQueries.CreateInstance(TournamentsListContext)
                                      .TryGetTournaments(LoginViewModel.CurrentUser.UserId,
                                                         out IQueryable<Tournament>? tournamentsCollection);

        switch (result)
        {
            case GetResult.Success:
                if (tournamentsCollection != null)
                {
                    TournamentsCollection =
                        new ObservableCollection<Tournament>(tournamentsCollection
                                                                .Include(t => t.Players));
                }

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

    public static Tournament? SelectedTournament { get; set; }

    public Tournament? SelectedTournamentObservable => SelectedTournament;

    public ICommand OpenTournamentCommand => _openTournamentCommand ??= new OpenTournamentCommand(this);

    public ObservableCollection<Tournament>? TournamentsCollection
    {
        get => _tournamentsCollection;
        set => SetField(ref _tournamentsCollection, value);
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e) =>
        OnPropertyChanged(nameof(SelectedTournamentObservable));
}
