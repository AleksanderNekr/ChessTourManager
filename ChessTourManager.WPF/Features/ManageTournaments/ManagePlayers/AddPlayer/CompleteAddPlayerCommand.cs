using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public class CompleteAddPlayerCommand : CommandBase
{
    private readonly AddPlayerViewModel _addPlayerViewModel;

    public CompleteAddPlayerCommand(AddPlayerViewModel addPlayerViewModel)
    {
        _addPlayerViewModel = addPlayerViewModel;
    }

    public override void Execute(object? parameter)
    {
        IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext)
                      .TryAddPlayer(out Player? player,
                                    TournamentsListViewModel.SelectedTournament!.TournamentId,
                                    LoginViewModel.CurrentUser!.UserId,
                                    _addPlayerViewModel.PlayerLastName.Trim(),
                                    _addPlayerViewModel.PlayerFirstName.Trim(),
                                    _addPlayerViewModel.Gender,
                                    teamId: _addPlayerViewModel.Team.TeamId);


        if (player is not null)
        {
            MessageBox.Show("Игрок успешно добавлен!", "Добавление игрока", MessageBoxButton.OK,
                            MessageBoxImage.Information);
            PlayerAddedEvent.OnPlayerAdded(new PlayerAddedEventArgs(player));
        }
    }
}
