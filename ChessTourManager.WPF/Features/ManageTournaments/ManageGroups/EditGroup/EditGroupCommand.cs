using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public class EditGroupCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is not Group group)
        {
            return;
        }

        EditGroupWindow editGroupWindow = new(group);
        editGroupWindow.ShowDialog();
    }
}
