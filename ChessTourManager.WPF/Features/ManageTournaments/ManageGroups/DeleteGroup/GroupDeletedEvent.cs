using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;

public delegate void GroupDeletedHandler(GroupDeletedEventArgs e);

public static class GroupDeletedEvent
{
    public static event GroupDeletedHandler? GroupDeleted;

    internal static void OnGroupDeleted(GroupDeletedEventArgs e)
    {
        GroupDeleted?.Invoke(e);
    }
}

public class GroupDeletedEventArgs : EventArgs
{
    public GroupDeletedEventArgs(Group group)
    {
        Group = group;
    }

    public Group Group { get; }
}
