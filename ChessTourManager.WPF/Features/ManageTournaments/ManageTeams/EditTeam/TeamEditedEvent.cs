using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public delegate void TeamChangedHandler(TeamChangedEventArgs e);

public static class TeamEditedEvent
{
    public static event TeamChangedHandler? TeamEdited;

    internal static void OnTeamChanged(TeamChangedEventArgs e)
    {
        TeamEdited?.Invoke(e);
    }
}

public class TeamChangedEventArgs : EventArgs
{
    public TeamChangedEventArgs(Team team)
    {
        Team = team;
    }

    public Team Team { get; }
}
