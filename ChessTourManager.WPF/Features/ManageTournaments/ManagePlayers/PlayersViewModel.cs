using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public class PlayersViewModel : ViewModelBase
{
    internal static readonly ChessTourContext PlayersContext = new();
    private                  ICommand?        _addPlayerCommand;
    private                  ICommand?        _deletePlayerCommand;

    private ObservableCollection<Player>? _playersCollection;

    public PlayersViewModel()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        PlayerAddedEvent.PlayerAdded           += PlayerAddedEvent_PlayerAdded;
        PlayerDeletedEvent.PlayerDeleted       += PlayerDeletedEvent_PlayerDeleted;
    }

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
        private set => SetField(ref _playersCollection, value);
    }

    public ICommand AddPlayerCommand => _addPlayerCommand ??= new AddPlayerCommand(this);

    public ICommand DeletePlayerCommand => _deletePlayerCommand ??= new DeletePlayerCommand(this);

    private void PlayerDeletedEvent_PlayerDeleted(PlayerDeletedEventArgs e) => UpdatePlayers();

    private void PlayerAddedEvent_PlayerAdded(PlayerAddedEventArgs e) => UpdatePlayers();

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e) => UpdatePlayers();

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
}
