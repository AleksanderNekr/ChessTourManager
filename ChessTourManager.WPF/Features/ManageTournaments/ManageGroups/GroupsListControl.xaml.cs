using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups;

public partial class GroupsListControl
{
    public GroupsListControl()
    {
        InitializeComponent();
        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        // Update tree view
        TreeView.Items.Refresh();
    }
}
