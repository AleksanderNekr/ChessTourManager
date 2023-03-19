using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;

namespace ChessTourManager.Domain.Algorithms;

public class RoundRobin : IRoundRobin
{
    public RoundRobin(ChessTourContext context, Tournament tournament)
    {
        IGetQueries.CreateInstance(context)
                   .TryGetGames(tournament.OrganizerId,
                                tournament.TournamentId,
                                out IEnumerable<Game>? games);
        if (games is not null)
        {
            GamesHistory = games.Select(g => (g.WhiteId, g.BlackId)).ToHashSet();
        }

        IGetQueries.CreateInstance(context)
                   .TryGetPlayers(tournament.OrganizerId, tournament.TournamentId,
                                  out IEnumerable<Player>? players);

        PlayersIds = players?.Select(p => p.PlayerId).ToList() ?? new List<int>();

        ConfigureTours();
    }

    private void ConfigureTours()
    {
        PlayersIds.Sort();
        if (PlayersIds.Count % 2 != 0)
        {
            PlayersIds.Add(-1);
        }

        for (var tourNumber = 1; tourNumber <= PlayersIds.Count / 2; tourNumber++)
        {
            _pairsForTour.TryAdd(tourNumber, ConfigurePairs());

            // Rotate players, so that the first player is always the same
            PlayersIds.Add(PlayersIds[0]);
            PlayersIds.RemoveAt(0);
        }
    }

    private readonly Dictionary<int, HashSet<(int, int)>> _pairsForTour = new();

    private HashSet<(int, int)> ConfigurePairs()
    {
        IEnumerable<int> whiteIds = PlayersIds.Take(PlayersIds.Count / 2);
        IEnumerable<int> blackIds = PlayersIds.Skip(PlayersIds.Count / 2).Reverse();

        return new HashSet<(int, int)>(whiteIds.Zip(blackIds, (w, b) => (w, b)));
    }

    private List<int> PlayersIds { get; }

    public IList<(int, int)> StartNewTour(int currentTour)
    {
        NewTourNumber = currentTour + 1;
        int tour = NewTourNumber;
        if (tour >= _pairsForTour.Count)
        {
            return new List<(int, int)>();
        }

        List<(int, int)> result = new();
        foreach ((int, int) pair in _pairsForTour[tour])
        {
            if (GamesHistory.Contains(pair))
            {
                tour++;
                continue;
            }

            result.Add(pair);
        }

        return result;
    }

    public HashSet<(int, int)>? GamesHistory { get; }

    public int NewTourNumber { get; private set; }
}
