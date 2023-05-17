using System;
using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;

public class DeleteGroupCommand : CommandBase
{
    private readonly ChessTourContext _context;

    public DeleteGroupCommand()
    {
        this._context = PlayersViewModel.PlayersContext;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is not Group group)
        {
            return;
        }

        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить группу {group.GroupName}?",
                                                  "Удаление группы", MessageBoxButton.YesNo,
                                                  MessageBoxImage.Question);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        try
        {
            this._context.Groups.Remove(group);
            this._context.SaveChanges();
            GroupDeletedEvent.OnGroupDeleted(this, new GroupDeletedEventArgs(group));
            MessageBox.Show($"Группа {group.GroupName} успешно удалена!", "Удаление группы",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Не удалось удалить группу! Возможно группа уже удалена. {e.Message}",
                            "Удаление группы", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
