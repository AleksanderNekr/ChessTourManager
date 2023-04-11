using System;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups;

public partial class GroupsListControl : IDisposable
{
    public GroupsListControl()
    {
        InitializeComponent();
        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        TreeView.Items.Refresh();
    }

    public void Dispose()
    {
        GroupChangedEvent.GroupChanged -= GroupChangedEvent_GroupChanged;
        ((IDisposable)DataContext).Dispose();
    }
}
