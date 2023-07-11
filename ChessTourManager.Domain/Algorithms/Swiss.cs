using System;
using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;

namespace ChessTourManager.Domain.Algorithms;

public class Swiss : IDrawingAlgorithm
{
    private readonly Tournament                           _tournament;
    private readonly ChessTourContext                     _context;
    private readonly HashSet<(int WhiteId, int BlackId)>  _gamesHistory;
    private readonly Dictionary<int, HashSet<(int, int)>> _pairsForTour = new();

    public Swiss(ChessTourContext context, Tournament tournament)
    {
        this._tournament = tournament;
        this._context    = context;
        IGetQueries.CreateInstance(context)
                   .TryGetGames(tournament.OrganizerId,
                                tournament.Id,
                                out List<Game>? games);
        if (games is not null)
        {
            this._gamesHistory = games.Select(static g => (g.WhiteId, g.BlackId)).ToHashSet();

            if (games.Any())
            {
                this.NewTourNumber = games.Max(static g => g.TourNumber) + 1;
            }
            else
            {
                this.NewTourNumber = 1;
            }
        }
        else
        {
            this._gamesHistory = new HashSet<(int, int)>();
            this.NewTourNumber = 1;
        }
    }

    private static IEnumerable<Player>? GetPlayers(ChessTourContext context, Tournament tournament)
    {
        IGetQueries.CreateInstance(context)
                   .TryGetPlayers(tournament.OrganizerId, tournament.Id,
                                  out List<Player>? players);

        return players;
    }

    private List<Player> ConfigureTour()
    {
        // Sort players by points and group them by points.
        Dictionary<double, List<Player>> groupedByPointsPlayers = GetPlayers(this._context, this._tournament)
                                                                 .OrderByDescending(static p => p.PointsAmount)
                                                                 .GroupBy(static p => p.PointsAmount)
                                                                 .ToDictionary(static g => g.Key,
                                                                               static g => g.ToList());
        // For each group (except last) if the amount of players in it is odd, then move the last player to the next group.
        for (var i = 0; i < groupedByPointsPlayers.Count - 1; i++)
        {
            if (groupedByPointsPlayers.ElementAt(i).Value.Count % 2 == 0)
            {
                continue;
            }

            groupedByPointsPlayers.ElementAt(i + 1).Value.Add(groupedByPointsPlayers.ElementAt(i).Value.Last());
            groupedByPointsPlayers.ElementAt(i).Value.RemoveAt(groupedByPointsPlayers.ElementAt(i).Value.Count - 1);
        }

        return groupedByPointsPlayers.SelectMany(static g => g.Value).ToList();
    }

    public IList<(int, int)> StartNewTour(int currentTour)
    {
        List<int> playersIds = this.ConfigureTour()
                                   .Select(static p => p.Id)
                                   .ToList();


        // Add a dummy player to make the number of players even.
        if (playersIds.Count % 2 != 0)
        {
            playersIds.Add(-1);
        }

        // Get pairs for current tour.
        HashSet<(int, int)> pairs = this.GetPairs(playersIds);

        // Add pairs to history.
        this._pairsForTour.Add(currentTour, pairs);

        // Return pairs.
        return pairs.ToList();
    }

    private HashSet<(int, int)> GetPairs(List<int> playersIds)
    {
        HashSet<(int, int)> pairs = new();

        while (playersIds.Count > 1)
        {
            for (var i = 0; i < playersIds.Count - 1; i++)
            {
                for (int j = i + 1; j < playersIds.Count; j++)
                {
                    // Check if the players have not already played against each other
                    if (this._gamesHistory.Contains((playersIds[i], playersIds[j])) ||
                        this._gamesHistory.Contains((playersIds[j], playersIds[i])))
                    {
                        continue;
                    }

                    // Swap colors for even tours.
                    pairs.Add(this.NewTourNumber % 2 == 0
                                  ? (playersIds[j], playersIds[i])
                                  : (playersIds[i], playersIds[j]));

                    // Remove players from the list.
                    playersIds.RemoveAt(j);
                    playersIds.RemoveAt(i);
                    i = 0;

                    break; // Move to the next player
                }
            }
        }

        return pairs;
    }

    public int NewTourNumber { get; }
}
