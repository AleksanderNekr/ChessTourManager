using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;

public class AddGroupCommand : CommandBase
{
    private readonly ManageGroupsViewModel _manageGroupsViewModel;

    public AddGroupCommand(ManageGroupsViewModel manageGroupsViewModel)
    {
        _manageGroupsViewModel = manageGroupsViewModel;
    }

    public override void Execute(object? parameter)
    {
        new AddGroupWindow().ShowDialog();
    }
}
