using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;

public static class PlayerDeletedEvent
{
    public delegate void PlayerAddedHandler(object source, PlayerDeletedEventArgs e);

    public static event PlayerAddedHandler? PlayerDeleted;

    internal static void OnPlayerDeleted(object source, PlayerDeletedEventArgs e)
    {
        PlayerDeleted?.Invoke(source,e);
    }
}

public class PlayerDeletedEventArgs : EventArgs
{
    public Player? DeletedPlayer;

    public PlayerDeletedEventArgs(Player? deletedPlayer)
    {
        this.DeletedPlayer = deletedPlayer;
    }
}
