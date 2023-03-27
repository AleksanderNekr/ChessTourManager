using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.DataAccess.Queries.Get;

internal class GetQueries : IGetQueries
{
    private static ChessTourContext _context = new();

    public GetQueries(ChessTourContext context)
    {
        _context = context;
    }

    public GetResult TryGetUserById(int id, out User? user)
    {
        IEnumerable<User> usersLocal = _context.Users.Include(u => u.Tournaments);
        user = usersLocal.FirstOrDefault(u => u.UserId == id);

        return user is { }
                   ? GetResult.Success
                   : GetResult.UserNotFound;
    }

    public GetResult TryGetUserByLoginAndPass(string? login, string password, out User? user)
    {
        string hash = PasswordHasher.HashPassword(password);
        user = _context.Users.FirstOrDefault(u => u.Email == login && u.PassHash == hash);
        return user is { }
                   ? GetResult.Success
                   : GetResult.UserNotFound;
    }

    public GetResult TryGetTournaments(int organiserId, out IEnumerable<Tournament>? tournaments)
    {
        if (TryGetUserById(organiserId, out User? organiser) == GetResult.Success)
        {
            tournaments = _context.Tournaments
                                  .Where(t => t.OrganizerId == organiserId)
                                  .Include(t => t.Kind)
                                  .Include(t => t.System);
            return GetResult.Success;
        }


        tournaments = default;
        return GetResult.UserNotFound;
    }

    public GetResult TryGetTournamentsWithTeamsAndPlayers(int organiserId, out IEnumerable<Tournament?>? tournaments)
    {
        if (TryGetUserById(organiserId, out User? _) == GetResult.Success)
        {
            tournaments = _context.Tournaments
                                  .Where(t => t.OrganizerId == organiserId)
                                  .Include(t => t.Teams)
                                  .ThenInclude(t => t.Players)
                                  .Include(t => t.Kind)
                                  .Include(t => t.System);

            return GetResult.Success;
        }

        tournaments = default;
        return GetResult.UserNotFound;
    }

    public GetResult TryGetPlayers(int organiserId, int tournamentId, out IEnumerable<Player>? players)
    {
        players = default;
        if (TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        IEnumerable<Tournament> tournaments = _context.Tournaments
                                                      .Where(t => t.OrganizerId == organiserId)
                                                      .Include(t => t.Players);

        Tournament? tournament = tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        players = tournament.Players;

        return GetResult.Success;
    }

    public GetResult TryGetPlayersWithTeamsAndGroups(int                      organiserId, int tournamentId,
                                                     out IEnumerable<Player>? players)
    {
        players = default;
        if (TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        IQueryable<Tournament> tournaments = _context.Tournaments
                                                     .Where(t => t.OrganizerId == organiserId)
                                                     .Include(t => t.Players)
                                                     .ThenInclude(p => p.Group);
        tournaments = tournaments.Include(t => t.Players)
                                 .ThenInclude(p => p.Team);


        Tournament? tournament = tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        players = tournament.Players;

        return GetResult.Success;
    }

    public GetResult TryGetTeamsWithPlayers(int organiserId, int tournamentId, out IEnumerable<Team>? teams)
    {
        teams = default;
        if (TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        IEnumerable<Tournament> tournaments = _context.Tournaments
                                                      .Where(t => t.OrganizerId == organiserId)
                                                      .Include(t => t.Teams)
                                                      .ThenInclude(t => t.Players);

        Tournament? tournament = tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        teams = tournament.Teams;

        return GetResult.Success;
    }

    public GetResult TryGetGroups(int                     organizerId, int tournamentId,
                                  out IEnumerable<Group>? groups)
    {
        groups = default;
        if (TryGetUserById(organizerId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        IEnumerable<Tournament?> tournaments = _context.Tournaments.Where(g => g.OrganizerId == organizerId);

        Tournament? tournament = tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        groups = _context.Groups
                         .Where(g => g.OrganizerId == organizerId && g.TournamentId == tournamentId)
                         .Include(g => g.Players);
        return GetResult.Success;
    }

    public GetResult TryGetGames(int organiserId, int tournamentId, out IEnumerable<Game>? games)
    {
        games = default;
        if (TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        Tournament? tournament = user?.Tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);

        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        games = _context.Games.Where(g => g.OrganizerId == organiserId && g.TournamentId == tournamentId)
                        .Include(g => g.PlayerBlack)
                        .Include(g => g.PlayerWhite);

        return GetResult.Success;
    }

    public GetResult GetKinds(out IEnumerable<Kind>? kinds)
    {
        kinds = _context.Kinds;
        return GetResult.Success;
    }

    public GetResult GetSystems(out IEnumerable<Entities.System>? systems)
    {
        systems = _context.Systems;
        return GetResult.Success;
    }
}

public enum GetResult
{
    Success,
    UserNotFound,
    NoTournaments,
    TournamentNotFound
}
