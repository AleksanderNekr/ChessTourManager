using System;
using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Queries;

internal class GetQueries : IGetQueries
{
    public bool TryGetUser(int id, out User? user)
    {
        user = ChessTourContext.CreateInstance().Users.Find(new User { UserId = id });

        return user is not null;
    }

    public bool TryGetTournaments(int organiserId, out IEnumerable<Tournament> tournaments)
    {
        tournaments = ChessTourContext.CreateInstance().Tournaments.Where(t => t.OrganizerId == organiserId);

        return true;
    }

    public bool TryGetPlayers(int organiserId, int tournamentId, out IEnumerable<Player> players)
    {
        players = Array.Empty<Player>();
        if (!TryGetUser(organiserId, out User? user) || user is null)
        {
            return false;
        }

        if (user.Tournaments.Count == 0)
        {
            return false;
        }

        players = ChessTourContext.CreateInstance().Players
                                  .Where(p => p.OrganizerId == organiserId && p.TournamentId == tournamentId);

        return true;
    }

    public bool TryGetTeams(int organiserId, int tournamentId, out IEnumerable<Team> teams)
    {
        teams = Array.Empty<Team>();
        if (!TryGetUser(organiserId, out User? user) || user is null)
        {
            return false;
        }

        if (user.Tournaments.Count == 0)
        {
            return false;
        }

        teams = ChessTourContext.CreateInstance().Teams
                                .Where(t => t.OrganizerId == organiserId && t.TournamentId == tournamentId);

        return true;
    }
}
