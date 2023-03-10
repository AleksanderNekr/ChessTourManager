using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;

public static class GameAddedEvent
{
    public delegate void GameAddedEventHandler(object sender, GameAddedEventArgs e);

    public static event GameAddedEventHandler? GameAdded;

    public static void OnGameAdded(object sender, GameAddedEventArgs e)
    {
        GameAdded?.Invoke(sender, e);
    }
}

public class GameAddedEventArgs : EventArgs
{
    public GameAddedEventArgs(Game game)
    {
        Game = game;
    }

    public Game Game { get; }
}
