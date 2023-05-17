using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;

public static class PlayerEditedEvent
{
    public delegate void PlayerEditedHandler(object source, PlayerEditedEventArgs e);

    public static event PlayerEditedHandler? PlayerEdited;

    internal static void OnPlayerEdited(object source, PlayerEditedEventArgs e)
    {
        PlayerEdited?.Invoke(source, e);
    }
}

public class PlayerEditedEventArgs : EventArgs
{
    public Player? EditedPlayer;

    public PlayerEditedEventArgs(Player? editedPlayer)
    {
        this.EditedPlayer = editedPlayer;
    }
}
