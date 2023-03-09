using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;

public class DeleteGroupCommand : CommandBase
{
    private readonly ChessTourContext _context;

    public DeleteGroupCommand()
    {
        _context = ManageGroupsViewModel.GroupsContext;
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

        _context.Groups.Remove(group);
        _context.SaveChanges();
        GroupDeletedEvent.OnGroupDeleted(new GroupDeletedEventArgs(group));
    }
}
