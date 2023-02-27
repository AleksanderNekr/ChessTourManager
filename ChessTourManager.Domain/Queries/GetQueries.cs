using System;
using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace ChessTourManager.Domain.Queries;

internal class GetQueries : IGetQueries
{
    public GetResult TryGetUser(int id, out User? user)
    {
        List<User> usersLocal = ChessTourContext.CreateInstance()
                                                .Users
                                                .Include(u => u.Tournaments)
                                                .ToList();
        user = usersLocal.Find(u => u.UserId == id);

        return user is not null
                   ? GetResult.Success
                   : GetResult.UserNotFound;
    }

    public GetResult TryGetTournaments(int organiserId, out IEnumerable<Tournament> tournaments)
    {
        if (TryGetUser(organiserId, out User? organiser) == GetResult.Success)
        {
            tournaments = organiser.Tournaments;
            return GetResult.Success;
        }


        tournaments = Array.Empty<Tournament>();
        return GetResult.UserNotFound;
    }

    public GetResult TryGetPlayers(int organiserId, int tournamentId, out IEnumerable<Player> players)
    {
        players = Array.Empty<Player>();
        if (TryGetUser(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user.Tournaments.Count == 0)
        {
            return GetResult.NoTournaments;
        }

        List<Tournament> tournaments = ChessTourContext.CreateInstance()
                                                       .Tournaments
                                                       .Where(t => t.OrganizerId == organiserId)
                                                       .Include(t => t.Players)
                                                       .ToList();

        Tournament? tournament = tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        players = tournament.Players;

        return GetResult.Success;
    }

    public GetResult TryGetTeams(int organiserId, int tournamentId, out IEnumerable<Team> teams)
    {
        teams = Array.Empty<Team>();
        if (TryGetUser(organiserId, out User? user) == GetResult.UserNotFound)
        {
            return GetResult.UserNotFound;
        }

        if (user.Tournaments.Count == 0)
        {
            return GetResult.NoTournaments;
        }

        List<Tournament> tournaments = ChessTourContext.CreateInstance()
                                                       .Tournaments
                                                       .Where(t => t.OrganizerId == organiserId)
                                                       .Include(t => t.Teams)
                                                       .ToList();

        Tournament? tournament = tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
        if (tournament is null)
        {
            return GetResult.TournamentNotFound;
        }

        teams = tournament.Teams;

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
