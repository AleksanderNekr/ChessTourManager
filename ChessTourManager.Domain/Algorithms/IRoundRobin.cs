using System.Collections.Generic;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Algorithms;

public interface IRoundRobin
{
    public HashSet<(int, int)>? GamesHistory { get; }

    public int NewTourNumber { get; }

    public static IRoundRobin Initialize(ChessTourContext context, Tournament? tournament)
    {
        return new RoundRobin(context, tournament);
    }

    public IList<(int, int)> StartNewTour(int currentTour);
}
