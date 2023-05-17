using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;

public static class GameAddedEvent
{
    public delegate void GameAddedEventHandler(object source, GameAddedEventArgs e);

    public static event GameAddedEventHandler? GameAdded;

    public static void OnGameAdded(object source, GameAddedEventArgs e)
    {
        GameAdded?.Invoke(source, e);
    }
}

public class GameAddedEventArgs : EventArgs
{
    public GameAddedEventArgs(Game? game)
    {
        this.Game = game;
    }

    public Game? Game { get; }
}
