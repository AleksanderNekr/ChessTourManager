﻿using System.Windows;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public class SaveGroupCommand : CommandBase
{
    private readonly EditGroupViewModel _editGroupViewModel;

    public SaveGroupCommand(EditGroupViewModel editGroupViewModel)
    {
        _editGroupViewModel = editGroupViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (_editGroupViewModel is { Group: null })
        {
            MessageBox.Show("Не удалось сохранить изменения. Группа не найдена.",
                            "Ошибка сохранения", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        MessageBoxResult result =
            MessageBox.Show($"Вы действительно хотите сохранить изменения в группе {_editGroupViewModel.Group.GroupName}?",
                            "Сохранение изменений", MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        _editGroupViewModel.Group.GroupName = _editGroupViewModel.GroupName;
        _editGroupViewModel.Group.Identity  = _editGroupViewModel.GroupIdentity;

        PlayersViewModel.PlayersContext.Groups.Update(_editGroupViewModel.Group);
        PlayersViewModel.PlayersContext.SaveChanges();

        GroupChangedEvent.OnGroupChanged(this, new GroupChangedEventArgs(_editGroupViewModel.Group));

        MessageBox.Show("Изменения в группе успешно сохранены!", "Сохранение изменений",
                        MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
