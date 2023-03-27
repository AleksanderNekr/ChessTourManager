using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;

namespace ChessTourManager.Domain.Algorithms;

public class RoundRobin : IRoundRobin
{
    private readonly ChessTourContext                     _context;
    private readonly Dictionary<int, HashSet<(int, int)>> _pairsForTour = new();

    private readonly Tournament? _tournament;

    private List<int> _playersIds;

    public RoundRobin(ChessTourContext context, Tournament? tournament)
    {
        _tournament = tournament;
        _context    = context;
        IGetQueries.CreateInstance(context)
                   .TryGetGames(tournament.OrganizerId,
                                tournament.TournamentId,
                                out IEnumerable<Game>? games);
        if (games is { })
        {
            IEnumerable<Game> gamesEnum = games.ToList();
            GamesHistory = gamesEnum.Select(g => (g.WhiteId, g.BlackId)).ToHashSet();

            if (gamesEnum.Any())
            {
                NewTourNumber = gamesEnum.Max(g => g.TourNumber) + 1;
            }
            else
            {
                NewTourNumber = 1;
            }
        }
        else
        {
            GamesHistory  = new HashSet<(int, int)>();
            NewTourNumber = 1;
        }

        IEnumerable<Player>? players = GetPlayers(context, tournament);

        _playersIds = GetPlayersIds(players);

        ConfigureTours();
    }

    public IList<(int, int)> StartNewTour(int currentTour)
    {
        ReconfigureIdsIfPlayersChanged();

        NewTourNumber = currentTour + 1;
        int tour = NewTourNumber;

        if (!CheckIfCanStart(tour))
        {
            return new List<(int, int)>();
        }

        List<(int, int)> result = GetPairsForTour(tour);

        return result;
    }

    private List<(int, int)> GetPairsForTour(int tour)
    {
        List<(int, int)> result = new();
        foreach ((int, int) pair in _pairsForTour[tour])
        {
            if (GamesHistory is { } && GamesHistory.Contains(pair))
            {
                tour++;
                continue;
            }

            result.Add(pair);
        }

        return result;
    }

    private bool CheckIfCanStart(int tourNumber)
    {
        bool areAllToursPlayed        = tourNumber        >= _pairsForTour.Count;
        bool isPlayersCountUnplayable = _playersIds.Count < 3;
        if (!(areAllToursPlayed || isPlayersCountUnplayable))
        {
            return true;
        }

        MessageBox.Show("Недостаточно игроков для проведения следующего тура.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        return false;
    }

    public HashSet<(int, int)>? GamesHistory { get; }

    public int NewTourNumber { get; private set; }

    private static IEnumerable<Player>? GetPlayers(ChessTourContext context, Tournament? tournament)
    {
        IGetQueries.CreateInstance(context)
                   .TryGetPlayers(tournament.OrganizerId, tournament.TournamentId,
                                  out IEnumerable<Player>? players);
        return players;
    }

    private static List<int> GetPlayersIds(IEnumerable<Player>? players)
    {
        return players?.Where(p => p.IsActive ?? false)
                       .Select(p => p.PlayerId).ToList() ?? new List<int>();
    }

    private void ConfigureTours()
    {
        _playersIds.Sort();

        // Add a dummy player to make the number of players even.
        if ((_playersIds.Count % 2) != 0)
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

    private HashSet<(int, int)> ConfigurePairs()
    {
        IEnumerable<int> whiteIds;
        IEnumerable<int> blackIds;

        // Swap colors for even tours.
        if ((NewTourNumber % 2) == 0)
        {
            whiteIds = _playersIds.Take(_playersIds.Count / 2);
            blackIds = _playersIds.Skip(_playersIds.Count / 2).Reverse();
        }
        else
        {
            whiteIds = _playersIds.Take(_playersIds.Count / 2).Reverse();
            blackIds = _playersIds.Skip(_playersIds.Count / 2);
        }

        return new HashSet<(int, int)>(whiteIds.Zip(blackIds, (w, b) => (w, b)));
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
}
