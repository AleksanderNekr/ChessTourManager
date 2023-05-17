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
        List<User> usersLocal = _context.Users.Include(u => u.Tournaments).ToList();
        user = usersLocal.FirstOrDefault(u => u.Id == id);

        return user is not null
                   ? GetResult.Success
                   : GetResult.UserNotFound;
    }

    public GetResult TryGetUserByLoginAndPass(string? login, string password, out User? user)
    {
        string hash = PasswordHasher.HashPassword(password);
        user = _context.Users.FirstOrDefault(u => u.Email == login && u.PasswordHash == hash);
        return user is not null
                   ? GetResult.Success
                   : GetResult.UserNotFound;
    }

    public GetResult TryGetTournaments(int organiserId, out List<Tournament>? tournaments)
    {
        if (this.TryGetUserById(organiserId, out User? organiser) == GetResult.Success)
        {
            tournaments = _context.Tournaments
                                  .Where(t => t.OrganizerId == organiserId)
                                  .Include(t => t.Kind)
                                  .Include(t => t.System)
                                  .ToList();
            return GetResult.Success;
        }


        tournaments = default(List<Tournament>);
        return GetResult.UserNotFound;
    }

    public GetResult TryGetTournamentsWithTeamsAndPlayers(int organiserId, out List<Tournament?>? tournaments)
    {
        if (this.TryGetUserById(organiserId, out User? _) == GetResult.Success)
        {
            tournaments = _context.Tournaments
                                  .Where(t => t.OrganizerId == organiserId)
                                  .Include(t => t.Teams)
                                  .ThenInclude(t => t.Players)
                                  .Include(t => t.Kind)
                                  .Include(t => t.System)
                                  .ToList();

            return GetResult.Success;
        }

        tournaments = default(List<Tournament?>);
        return GetResult.UserNotFound;
    }

    public GetResult TryGetPlayers(int organiserId, int tournamentId, out List<Player>? players)
    {
        players = default(List<Player>);
        if (this.TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        List<Tournament> tournaments = _context.Tournaments
                                               .Where(t => t.OrganizerId == organiserId)
                                               .Include(t => t.Players)
                                               .ToList();

        Tournament? tournament = tournaments.FirstOrDefault(t => t.Id == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        players = tournament.Players.ToList();

        return GetResult.Success;
    }

    public GetResult TryGetPlayersWithTeamsAndGroups(int               organiserId, int tournamentId,
                                                     out List<Player>? players)
    {
        players = default(List<Player>);
        if (this.TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
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


        Tournament? tournament = tournaments.FirstOrDefault(t => t.Id == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        players = tournament.Players.ToList();

        return GetResult.Success;
    }

    public GetResult TryGetTeamsWithPlayers(int organiserId, int tournamentId, out List<Team>? teams)
    {
        teams = default(List<Team>);
        if (this.TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        List<Tournament> tournaments = _context.Tournaments
                                               .Where(t => t.OrganizerId == organiserId)
                                               .Include(t => t.Teams)
                                               .ThenInclude(t => t.Players)
                                               .ToList();

        Tournament? tournament = tournaments.FirstOrDefault(t => t.Id == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        teams = tournament.Teams.ToList();

        return GetResult.Success;
    }

    public GetResult TryGetGroups(int              organizerId, int tournamentId,
                                  out List<Group>? groups)
    {
        groups = default(List<Group>);
        if (this.TryGetUserById(organizerId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        List<Tournament?> tournaments = _context.Tournaments
                                                .Where(g => g.OrganizerId == organizerId)
                                                .ToList();

        Tournament? tournament = tournaments.FirstOrDefault(t => t.Id == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        groups = _context.Groups
                         .Where(g => g.OrganizerId == organizerId && g.TournamentId == tournamentId)
                         .Include(g => g.Players)
                         .ToList();
        return GetResult.Success;
    }

    public GetResult TryGetGames(int organiserId, int tournamentId, out List<Game>? games)
    {
        games = default(List<Game>);
        if (this.TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user is { Tournaments.Count: 0 })
        {
            return GetResult.NoTournaments;
        }

        Tournament? tournament = user?.Tournaments.FirstOrDefault(t => t.Id == tournamentId);

        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        games = _context.Games.Where(g => g.OrganizerId == organiserId && g.TournamentId == tournamentId)
                        .Include(g => g.PlayerBlack)
                        .Include(g => g.PlayerWhite)
                        .ToList();

        return GetResult.Success;
    }

    public GetResult GetKinds(out List<Kind>? kinds)
    {
        kinds = _context.Kinds.ToList();
        return GetResult.Success;
    }

    public GetResult GetSystems(out List<Entities.System>? systems)
    {
        systems = _context.Systems.ToList();
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
