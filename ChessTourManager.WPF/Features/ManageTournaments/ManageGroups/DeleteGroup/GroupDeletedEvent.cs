using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;

public static class GroupDeletedEvent
{
    public delegate void GroupDeletedHandler(object source, GroupDeletedEventArgs e);

    public static event GroupDeletedHandler? GroupDeleted;

    internal static void OnGroupDeleted(object source, GroupDeletedEventArgs e)
    {
        GroupDeleted?.Invoke(source, e);
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
