using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;

public static class TeamAddedEvent
{
    public delegate void TeamAddedHandler(object source, TeamAddedEventArgs e);

    public static event TeamAddedHandler? TeamAdded;

    internal static void OnTeamAdded(object source, TeamAddedEventArgs e)
    {
        TeamAdded?.Invoke(source,e);
    }
}

public class TeamAddedEventArgs : EventArgs
{
    public TeamAddedEventArgs(Team team)
    {
        this.Team = team;
    }

    public Team Team { get; }
}
