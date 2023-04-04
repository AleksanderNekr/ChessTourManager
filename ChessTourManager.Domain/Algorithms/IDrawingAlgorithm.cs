using System.Collections.Generic;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Algorithms;

public interface IDrawingAlgorithm
{
    public static IDrawingAlgorithm Initialize(ChessTourContext context, Tournament tournament) =>
        new RoundRobin(context, tournament);

    public IList<(int, int)> StartNewTour(int currentTour);

    public HashSet<(int, int)>? GamesHistory { get; }

    public int NewTourNumber { get; }
}
