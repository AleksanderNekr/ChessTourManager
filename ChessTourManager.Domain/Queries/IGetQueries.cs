using System.Collections.Generic;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Queries;

public interface IGetQueries
{
    public bool TryGetUser(int id, out User? user);

    public bool TryGetTournaments(int organiserId, out IEnumerable<Tournament> tournaments);

    public bool TryGetPlayers(int organiserId, int tournamentId, out IEnumerable<Player> players);

    public bool TryGetTeams(int organiserId, int tournamentId, out IEnumerable<Team> teams);
}
