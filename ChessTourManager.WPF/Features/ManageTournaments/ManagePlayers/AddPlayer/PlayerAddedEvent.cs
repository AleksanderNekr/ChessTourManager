using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public static class PlayerAddedEvent
{
    public delegate void PlayerAddedHandler(object source,PlayerAddedEventArgs e);

    public static event PlayerAddedHandler? PlayerAdded;

    internal static void OnPlayerAdded(object source,PlayerAddedEventArgs e)
    {
        PlayerAdded?.Invoke(source,e);
    }
}

public class PlayerAddedEventArgs : EventArgs
{
    public Player? AddedPlayer;

    public PlayerAddedEventArgs(Player? addedPlayer)
    {
        AddedPlayer = addedPlayer;
    }
}
