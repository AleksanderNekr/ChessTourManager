using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;

public class AddGroupCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        new AddGroupWindow().ShowDialog();
    }
}
