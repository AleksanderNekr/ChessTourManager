using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;

public delegate void GroupAddedHandler(GroupAddedEventArgs e);

public static class GroupAddedEvent
{
    public static event GroupAddedHandler? GroupAdded;

    internal static void OnGroupAdded(GroupAddedEventArgs e)
    {
        GroupAdded?.Invoke(e);
    }
}

public class GroupAddedEventArgs : EventArgs
{
    public GroupAddedEventArgs(Group group)
    {
        Group = group;
    }

    public Group Group { get; }
}
