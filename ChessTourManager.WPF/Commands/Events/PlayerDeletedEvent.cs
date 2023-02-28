using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Commands.Events;

public class PlayerDeletedEvent
{
    /// <summary>
    ///     Delegate on handling UserSuccessLogin event.
    /// </summary>
    public delegate void PlayerAddedHandler(PlayerDeletedEventArgs e);

    public static event PlayerAddedHandler? PlayerDeleted;

    internal static void OnPlayerDeleted(PlayerDeletedEventArgs e) => PlayerDeleted?.Invoke(e);
}

public class PlayerDeletedEventArgs : EventArgs
{
    public Player? DeletedPlayer;

    public PlayerDeletedEventArgs(Player? deletedPlayer) => DeletedPlayer = deletedPlayer;
}
