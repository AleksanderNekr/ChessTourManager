using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;

public delegate void PlayerEditedHandler(PlayerEditedEventArgs e);

public static class PlayerEditedEvent
{
    public static event PlayerEditedHandler? PlayerEdited;

    internal static void OnPlayerEdited(PlayerEditedEventArgs e)
    {
        PlayerEdited?.Invoke(e);
    }
}

public class PlayerEditedEventArgs : EventArgs
{
    public Player? EditedPlayer;

    public PlayerEditedEventArgs(Player? editedPlayer)
    {
        EditedPlayer = editedPlayer;
    }
}
