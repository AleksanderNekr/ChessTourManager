using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.Domain.Queries;

internal class GetQueries : IGetQueries
{
    public GetResult TryGetUserById(int id, out User? user)
    {
        IEnumerable<User> usersLocal = ChessTourContext.CreateInstance()
                                                       .Users
                                                       .Include(u => u.Tournaments.AsQueryable());
        user = usersLocal.FirstOrDefault(u => u.UserId == id);

        return user is not null
                   ? GetResult.Success
                   : GetResult.UserNotFound;
    }

    public GetResult TryGetUserByLoginAndPass(string login, string password, out User? user)
    {
        string hash = PasswordHasher.HashPassword(password);
        user = ChessTourContext.CreateInstance().Users
                               .FirstOrDefault(u => u.Email == login
                                                 && PasswordHasher.VerifyPassword(password, hash));
        return user is not null
                   ? GetResult.Success
                   : GetResult.UserNotFound;
    }

    public GetResult TryGetTournaments(int organiserId, out IQueryable<Tournament>? tournaments)
    {
        if (TryGetUserById(organiserId, out User? organiser) == GetResult.Success)
        {
            tournaments = organiser.Tournaments.AsQueryable();
            return GetResult.Success;
        }


        tournaments = default;
        return GetResult.UserNotFound;
    }

    public GetResult TryGetPlayers(int organiserId, int tournamentId, out IQueryable<Player>? players)
    {
        players = default;
        if (TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user.Tournaments.Count == 0)
        {
            return GetResult.NoTournaments;
        }

        IQueryable<Tournament> tournaments = ChessTourContext.CreateInstance()
                                                             .Tournaments
                                                             .Where(t => t.OrganizerId == organiserId)
                                                             .Include(t => t.Players);

        Tournament? tournament = tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        players = tournament.Players.AsQueryable();

        return GetResult.Success;
    }

    public GetResult TryGetTeams(int organiserId, int tournamentId, out IQueryable<Team>? teams)
    {
        teams = default;
        if (TryGetUserById(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user.Tournaments.Count == 0)
        {
            return GetResult.NoTournaments;
        }

        IQueryable<Tournament> tournaments = ChessTourContext.CreateInstance()
                                                             .Tournaments
                                                             .Where(t => t.OrganizerId == organiserId)
                                                             .Include(t => t.Teams);

        Tournament? tournament = tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        teams = tournament.Teams.AsQueryable();

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
