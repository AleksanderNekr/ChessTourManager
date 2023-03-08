using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;

public delegate void TeamAddedHandler(TeamAddedEventArgs e);

public class TeamAddedEvent
{
    public static event TeamAddedHandler? TeamAdded;

    internal static void OnTeamAdded(TeamAddedEventArgs e)
    {
        TeamAdded?.Invoke(e);
    }
}

public class TeamAddedEventArgs : EventArgs
{
    public TeamAddedEventArgs(Team team)
    {
        Team = team;
    }

    public Team Team { get; }
}
