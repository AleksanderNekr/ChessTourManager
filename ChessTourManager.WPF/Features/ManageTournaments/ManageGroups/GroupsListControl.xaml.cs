using System;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups;

public partial class GroupsListControl : IDisposable
{
    public GroupsListControl()
    {
        this.InitializeComponent();
        GroupChangedEvent.GroupChanged += this.GroupChangedEvent_GroupChanged;
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        this.TreeView.Items.Refresh();
    }

    public void Dispose()
    {
        GroupChangedEvent.GroupChanged -= this.GroupChangedEvent_GroupChanged;
        ((IDisposable)this.DataContext).Dispose();
    }
}
