using System.Collections.Generic;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Algorithms;

public interface IRoundRobin
{
    public static IRoundRobin Initialize(ChessTourContext context, Tournament tournament) =>
        new RoundRobin(context, tournament);

    public IEnumerable<(int, int)> StartNewTour(int currentTour);

    public HashSet<(int, int)> GamesHistory { get; }

    public int NewTourNumber { get; }
}
