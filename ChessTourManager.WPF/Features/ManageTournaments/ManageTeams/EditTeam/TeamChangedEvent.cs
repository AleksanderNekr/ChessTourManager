using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public static class TeamChangedEvent
{
    public delegate void TeamChangedHandler(object source, TeamChangedEventArgs e);

    public static event TeamChangedHandler? TeamEdited;

    internal static void OnTeamChanged(object source, TeamChangedEventArgs e)
    {
        TeamEdited?.Invoke(source,e);
    }
}

public class TeamChangedEventArgs : EventArgs
{
    public TeamChangedEventArgs(Team? team)
    {
        Team = team;
    }

    public Team? Team { get; }
}
