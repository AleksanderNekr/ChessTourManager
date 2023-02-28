using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Commands.Events;

public class PlayerAddedEvent
{
    /// <summary>
    ///     Delegate on handling UserSuccessLogin event.
    /// </summary>
    public delegate void PlayerAddedHandler(PlayerAddedEventArgs e);

    public static event PlayerAddedHandler? PlayerAdded;
    internal static void                    OnPlayerAdded(PlayerAddedEventArgs e) => PlayerAdded?.Invoke(e);
}

public class PlayerAddedEventArgs : EventArgs
{
    public Player? AddedPlayer;

    public PlayerAddedEventArgs(Player? addedPlayer) => AddedPlayer = addedPlayer;
}
