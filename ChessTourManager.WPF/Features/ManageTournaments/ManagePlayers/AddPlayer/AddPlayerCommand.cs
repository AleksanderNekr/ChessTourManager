using System;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public class AddPlayerCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (MainViewModel.SelectedTournament == null)
        {
            return;
        }

        InsertResult result = IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext)
                                            .TryAddPlayer(out Player? player,
                                                          MainViewModel.SelectedTournament.Id,
                                                          MainViewModel.SelectedTournament.OrganizerId,
                                                          string.Empty, string.Empty);
        if (result == InsertResult.Fail)
        {
            MessageBox.Show("Не удалось добавить игрока! Возможно игрок с такими данными уже существует",
                            "Добавление игрока", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        PlayerAddedEvent.OnPlayerAdded(this, new PlayerAddedEventArgs(player));
    }
}
