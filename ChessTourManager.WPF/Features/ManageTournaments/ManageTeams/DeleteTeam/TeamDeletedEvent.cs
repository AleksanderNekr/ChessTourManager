using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;

public delegate void TeamDeletedHandler(TeamDeletedEventArgs e);

public class TeamDeletedEvent
{
    public static event TeamDeletedHandler? TeamDeleted;

    internal static void OnTeamDeleted(TeamDeletedEventArgs e)
    {
        TeamDeleted?.Invoke(e);
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
