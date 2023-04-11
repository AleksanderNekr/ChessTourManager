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
        _tournament = tournament;
        _context    = context;
        IGetQueries.CreateInstance(context)
                   .TryGetGames(tournament.OrganizerId,
                                tournament.TournamentId,
                                out List<Game>? games);
        if (games is not null)
        {
            _gamesHistory = games.Select(g => (g.WhiteId, g.BlackId)).ToHashSet();

            if (games.Any())
            {
                NewTourNumber = games.Max(g => g.TourNumber) + 1;
            }
            else
            {
                NewTourNumber = 1;
            }
        }
        else
        {
            _gamesHistory = new HashSet<(int, int)>();
            NewTourNumber = 1;
        }

        List<Player>? players = GetPlayers(context, tournament);

        _playersIds = GetPlayersIds(players);

        ConfigureTours();
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
        _playersIds.Sort();

        // Add a dummy player to make the number of players even.
        if (_playersIds.Count % 2 != 0)
        {
            _playersIds.Add(-1);
        }

        for (var tourNumber = 1; tourNumber <= _playersIds.Count; tourNumber++)
        {
            if (_pairsForTour.ContainsKey(tourNumber))
            {
                _pairsForTour[tourNumber] = ConfigurePairs();
            }
            else
            {
                _pairsForTour.Add(tourNumber, ConfigurePairs());
            }

            // Rotate players, so that the first player is always the same.
            _playersIds.Add(_playersIds[0]);
            _playersIds.RemoveAt(0);
        }
    }

    private readonly Dictionary<int, HashSet<(int, int)>> _pairsForTour = new();

    private HashSet<(int, int)> ConfigurePairs()
    {
        List<int> whiteIds;
        List<int> blackIds;

        // Swap colors for even tours.
        if (NewTourNumber % 2 == 0)
        {
            whiteIds = _playersIds.Take(_playersIds.Count / 2).ToList();
            blackIds = _playersIds.Skip(_playersIds.Count / 2).Reverse().ToList();
        }
        else
        {
            whiteIds = _playersIds.Take(_playersIds.Count / 2).Reverse().ToList();
            blackIds = _playersIds.Skip(_playersIds.Count / 2).ToList();
        }

        return new HashSet<(int, int)>(whiteIds.Zip(blackIds, (w, b) => (w, b)));
    }

    private List<int> _playersIds;

    public IList<(int, int)> StartNewTour(int currentTour)
    {
        ReconfigureIdsIfPlayersChanged();

        NewTourNumber = currentTour + 1;
        int tour = NewTourNumber;
        if (tour >= _pairsForTour.Count || _playersIds.Count < 3)
        {
            return new List<(int, int)>();
        }

        List<(int, int)> result = new();
        foreach ((int, int) pair in _pairsForTour[tour])
        {
            if (_gamesHistory.Contains(pair))
            {
                tour++;
                continue;
            }

            result.Add(pair);
        }

        return result;
    }

    private void ReconfigureIdsIfPlayersChanged()
    {
        // If players changed, reconfigure the tours.
        List<int> ids = GetPlayersIds(GetPlayers(_context, _tournament));
        if (ids.Count != _playersIds.Count)
        {
            _playersIds = ids;
            ConfigureTours();
            return;
        }

        for (var i = 0; i < _playersIds.Count; i++)
        {
            if (_playersIds[i] != ids[i])
            {
                _playersIds = ids;
                ConfigureTours();
                break;
            }
        }
    }

    private readonly HashSet<(int, int)>? _gamesHistory;

    public int NewTourNumber { get; private set; }
}
