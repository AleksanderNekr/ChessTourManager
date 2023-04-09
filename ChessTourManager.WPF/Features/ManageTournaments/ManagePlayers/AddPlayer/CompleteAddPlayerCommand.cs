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
        if (MainViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return;
        }

        InsertResult result = IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext)
                                            .TryAddPlayer(out Player? player,
                                                          MainViewModel.SelectedTournament.TournamentId,
                                                          LoginViewModel.CurrentUser.UserId,
                                                          _addPlayerViewModel.PlayerLastName.Trim(),
                                                          _addPlayerViewModel.PlayerFirstName.Trim(),
                                                          _addPlayerViewModel.Gender,
                                                          teamId: _addPlayerViewModel.Team?.TeamId);


        if (result == InsertResult.Fail)
        {
            MessageBox.Show("Не удалось добавить игрока! Возможно игрок с такими данными уже существует",
                            "Добавление игрока", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MessageBox.Show("Игрок успешно добавлен!", "Добавление игрока", MessageBoxButton.OK,
                        MessageBoxImage.Information);
        PlayerAddedEvent.OnPlayerAdded(this, new PlayerAddedEventArgs(player));
    }
}
