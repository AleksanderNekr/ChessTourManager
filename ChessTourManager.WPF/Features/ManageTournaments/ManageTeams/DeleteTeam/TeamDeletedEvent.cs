using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;

public delegate void TeamDeletedHandler(object source, TeamDeletedEventArgs e);

public static class TeamDeletedEvent
{
    public static event TeamDeletedHandler? TeamDeleted;

    internal static void OnTeamDeleted(object source, TeamDeletedEventArgs e)
    {
        TeamDeleted?.Invoke(source,e);
    }
}

public class TeamDeletedEventArgs : EventArgs
{
    public TeamDeletedEventArgs(Team team)
    {
        Team = team;
    }

    public Team Team { get; }
}
