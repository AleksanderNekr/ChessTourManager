using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;

namespace ChessTourManager.Domain.Algorithms;

public class RoundRobin : IDrawingAlgorithm
{
    public RoundRobin(ChessTourContext context, Tournament tournament)
    {
        this._tournament = tournament;
        this._context       = context;
        IGetQueries.CreateInstance(context)
                   .TryGetGames(tournament.OrganizerId,
                                tournament.TournamentId,
                                out List<Game>? games);
        if (games is not null)
        {
            this._gamesHistory = games.Select(g => (g.WhiteId, g.BlackId)).ToHashSet();

            if (games.Any())
            {
                this.NewTourNumber = games.Max(g => g.TourNumber) + 1;
            }
            else
            {
                this.NewTourNumber = 1;
            }
        }
        else
        {
            this._gamesHistory = new HashSet<(int, int)>();
            this.NewTourNumber    = 1;
        }

        List<Player>? players = GetPlayers(context, tournament);

        this._playersIds = GetPlayersIds(players);

        this.ConfigureTours();
    }

    private static List<Player>? GetPlayers(ChessTourContext context, Tournament tournament)
    {
        IGetQueries.CreateInstance(context)
                   .TryGetPlayers(tournament.OrganizerId, tournament.TournamentId,
                                  out List<Player>? players);
        return players;
    }

    private static List<int> GetPlayersIds(List<Player>? players)
    {
        return players?.Where(p => p.IsActive ?? false)
                       .Select(p => p.PlayerId).ToList() ?? new List<int>();
    }

    private readonly Tournament       _tournament;
    private readonly ChessTourContext _context;

    private void ConfigureTours()
    {
        this._playersIds.Sort();

        // Add a dummy player to make the number of players even.
        if (this._playersIds.Count % 2 != 0)
        {
            this._playersIds.Add(-1);
        }

        for (var tourNumber = 1; tourNumber <= this._playersIds.Count; tourNumber++)
        {
            if (this._pairsForTour.ContainsKey(tourNumber))
            {
                this._pairsForTour[tourNumber] = this.ConfigurePairs();
            }
            else
            {
                this._pairsForTour.Add(tourNumber, this.ConfigurePairs());
            }

            // Rotate players, so that the first player is always the same.
            this._playersIds.Add(this._playersIds[1]);
            this._playersIds.RemoveAt(1);
        }
    }

    private readonly Dictionary<int, HashSet<(int, int)>> _pairsForTour = new();

    private HashSet<(int, int)> ConfigurePairs()
    {
        List<int> whiteIds;
        List<int> blackIds;

        // Swap colors for even tours.
        if (this.NewTourNumber % 2 == 0)
        {
            whiteIds = this._playersIds.Take(this._playersIds.Count / 2).ToList();
            blackIds = this._playersIds.Skip(this._playersIds.Count    / 2).Reverse().ToList();
            return new HashSet<(int, int)>(whiteIds.Zip(blackIds, (w, b) => (w, b)));
        }

        whiteIds = this._playersIds.Take(this._playersIds.Count / 2).Reverse().ToList();
        blackIds = this._playersIds.Skip(this._playersIds.Count    / 2).ToList();
        return new HashSet<(int, int)>(whiteIds.Zip(blackIds, (w, b) => (b, w)));
    }

    private List<int> _playersIds;

    public IList<(int, int)> StartNewTour(int currentTour)
    {
        this.ReconfigureIdsIfPlayersChanged();

        this.NewTourNumber = currentTour + 1;
        int tour = this.NewTourNumber;
        if (tour >= this._pairsForTour.Count || this._playersIds.Count < 3)
        {
            return new List<(int, int)>();
        }

        List<(int, int)> result = new();
        foreach ((int, int) pair in this._pairsForTour[tour])
        {
            if (this._gamesHistory.Contains(pair) || this._gamesHistory.Contains((pair.Item2, pair.Item1)))
            {
                tour++;
                continue;
            }

            result.Add(pair);
            this._gamesHistory.Add(pair);
        }

        return result;
    }

    private void ReconfigureIdsIfPlayersChanged()
    {
        // If players changed, reconfigure the tours.
        List<int> ids = GetPlayersIds(GetPlayers(this._context, this._tournament));
        if (ids.Count != this._playersIds.Count)
        {
            this._playersIds = ids;
            this.ConfigureTours();
            return;
        }

        for (var i = 0; i < this._playersIds.Count; i++)
        {
            if (this._playersIds[i] != ids[i])
            {
                this._playersIds = ids;
                this.ConfigureTours();
                break;
            }
        }
    }

    private readonly HashSet<(int, int)>? _gamesHistory;

    public int NewTourNumber { get; private set; }
}
