using System;
using System.Windows;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;

public class CompleteEditPlayerCommand : CommandBase
{
    private readonly EditPlayerViewModel _editPlayerViewModel;

    public CompleteEditPlayerCommand(EditPlayerViewModel editPlayerViewModel)
    {
        _editPlayerViewModel = editPlayerViewModel;
    }

    public override void Execute(object? parameter)
    {
        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите подтвердить редактирование игрока?",
                                                  "Редактирование игрока",
                                                  MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        if (_editPlayerViewModel is { Player: null })
        {
            return;
        }

        try
        {
            _editPlayerViewModel.Player.PlayerFirstName = _editPlayerViewModel.PlayerFirstName?.Trim();
            _editPlayerViewModel.Player.PlayerLastName  = _editPlayerViewModel.PlayerLastName?.Trim();
            _editPlayerViewModel.Player.Gender          = _editPlayerViewModel.Gender;

            PlayersViewModel.PlayersContext.Players.Update(_editPlayerViewModel.Player);

            PlayersViewModel.PlayersContext.SaveChanges();

            MessageBox.Show("Игрок успешно отредактирован!", "Редактирование игрока",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            PlayerEditedEvent.OnPlayerEdited(this, new PlayerEditedEventArgs(_editPlayerViewModel.Player));
        }
        catch (Exception e)
        {
            MessageBox.Show("Не удалось отредактировать игрока!\n" + e.Message, "Редактирование игрока",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
