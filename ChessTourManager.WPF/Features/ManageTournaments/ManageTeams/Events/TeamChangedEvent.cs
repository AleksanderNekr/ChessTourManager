using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.Events;

public delegate void TeamChangedHandler(TeamChangedEventArgs e);

public class TeamChangedEvent
{
    public static event TeamChangedHandler? TeamChanged;

    internal static void OnTeamChanged(TeamChangedEventArgs e)
    {
        TeamChanged?.Invoke(e);
    }
}

public class TeamChangedEventArgs : EventArgs
{
    public TeamChangedEventArgs(Player forPlayer)
    {
        ForPlayer = forPlayer;
    }

    public Player ForPlayer { get; }
}
