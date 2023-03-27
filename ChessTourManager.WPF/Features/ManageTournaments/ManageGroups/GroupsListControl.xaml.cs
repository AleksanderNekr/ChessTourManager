using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups;

public partial class GroupsListControl
{
    public GroupsListControl()
    {
        InitializeComponent();
        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
    }

    private void GroupChangedEvent_GroupChanged(GroupChangedEventArgs e)
    {
        // Update tree view
        TreeView.Items.Refresh();
    }
}
