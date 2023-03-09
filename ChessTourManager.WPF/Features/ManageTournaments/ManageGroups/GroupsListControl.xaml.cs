using System.Windows.Controls;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups;

public partial class GroupsListControl : UserControl
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
