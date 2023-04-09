using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public static class GroupChangedEvent
{
    public delegate void GroupChangedHandler(object source, GroupChangedEventArgs e);

    public static event GroupChangedHandler? GroupChanged;

    internal static void OnGroupChanged(object source, GroupChangedEventArgs e)
    {
        GroupChanged?.Invoke(source,e);
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
