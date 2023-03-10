using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public static class ResultChangedEvent
{
    public delegate void ResultChangedEventHandler(object? sender, ResultChangedEventArgs e);

    public static event ResultChangedEventHandler? ResultChanged;

    public static void OnResultChanged(object? sender, ResultChangedEventArgs e)
    {
        ResultChanged?.Invoke(sender, e);
    }
}

public class ResultChangedEventArgs : EventArgs
{
    public ResultChangedEventArgs(Game? game)
    {
        Game = game;
    }

    public Game? Game { get; }
}
