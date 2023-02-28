using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries;
using ChessTourManager.WPF.Commands;
using ChessTourManager.WPF.Commands.Events;

namespace ChessTourManager.WPF.ViewModels;

public class PlayersViewModel : ViewModelBase
{
    internal static readonly ChessTourContext PlayersContext = new();

    public PlayersViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        PlayerAddedEvent.PlayerAdded           += PlayerAddedEvent_PlayerAdded;
        PlayerDeletedEvent.PlayerDeleted       += PlayerDeletedEvent_PlayerDeleted;
    }

    private void PlayerDeletedEvent_PlayerDeleted(PlayerDeletedEventArgs e)
    {
        UpdatePlayers();
    }

    private void PlayerAddedEvent_PlayerAdded(PlayerAddedEventArgs e)
    {
        UpdatePlayers();
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        UpdatePlayers();
    }

    private void UpdatePlayers()
    {
        IGetQueries.CreateInstance(PlayersContext)
                   .TryGetPlayers(LoginViewModel.CurrentUser.UserId,
                                  TournamentsListViewModel.SelectedTournament.TournamentId,
                                  out IQueryable<Player>? players);

        if (players != null)
        {
            PlayersCollection = new ObservableCollection<Player>(players);
        }
    }

    private ObservableCollection<Player>? _playersCollection;
    private ICommand?                     _addPlayerCommand;
    private ICommand                      _deletePlayerCommand;

    public ObservableCollection<Player>? PlayersCollection
    {
        get
        {
            if (_playersCollection != null)
            {
                return _playersCollection;
            }

            if (TournamentsListViewModel.SelectedTournament == null)
            {
                return new ObservableCollection<Player>();
            }

            return _playersCollection;
        }
        private set { SetField(ref _playersCollection, value); }
    }

    public ICommand AddPlayerCommand => _addPlayerCommand ??= new AddPlayerCommand(this);

    public ICommand DeletePlayerCommand => _deletePlayerCommand ??= new DeletePlayerCommand(this);
}
