using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public delegate void GroupChangedHandler(GroupChangedEventArgs e);

public static class GroupChangedEvent
{
    public static event GroupChangedHandler? GroupChanged;

    internal static void OnGroupChanged(GroupChangedEventArgs e)
    {
        GroupChanged?.Invoke(e);
    }
}

public class GroupChangedEventArgs : EventArgs
{
    public GroupChangedEventArgs(Group? group)
    {
        Group = group;
    }

    public Group? Group { get; }
}
