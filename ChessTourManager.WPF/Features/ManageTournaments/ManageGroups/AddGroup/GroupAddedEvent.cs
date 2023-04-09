using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;

public static class GroupAddedEvent
{
    public delegate void GroupAddedHandler(object source, GroupAddedEventArgs e);

    public static event GroupAddedHandler? GroupAdded;

    internal static void OnGroupAdded(object source, GroupAddedEventArgs e)
    {
        GroupAdded?.Invoke(source, e);
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
