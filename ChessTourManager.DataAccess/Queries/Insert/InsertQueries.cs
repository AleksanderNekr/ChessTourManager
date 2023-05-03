using System;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.DataAccess.Queries.Insert;

internal class InsertQueries : IInsertQueries
{
    private static ChessTourContext _context = new();

    public InsertQueries(ChessTourContext context)
    {
        _context = context;
    }

    public InsertResult TryAddUser(out User? user, string lastName, string firstName, string email, string password,
                                   string    patronymic       = "-",
                                   int       tournamentsLimit = 50)
    {
        user = new User
               {
                   UserLastName   = lastName,
                   UserFirstName  = firstName,
                   Email          = email,
                   PasswordHash       = PasswordHasher.HashPassword(password),
                   UserPatronymic = patronymic,
                   TournamentsLim = tournamentsLimit,
                   RegisterDate   = DateOnly.FromDateTime(DateTime.UtcNow),
                   RegisterTime   = TimeOnly.FromDateTime(DateTime.UtcNow)
               };
        try
        {
            _context.Entry(user).State = EntityState.Detached;
            _context.Users.Add(user);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception)
        {
            _context.Entry(user).State = EntityState.Detached;
            user                       = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddTournament(out Tournament? addedTournament, int organiserId, string? tournamentName,
                                         int             systemId,        int kindId,
                                         int             toursCount          = 7,
                                         string?         place               = "-",
                                         DateOnly?       tournamentDateStart = null,
                                         TimeOnly?       tournamentTimeStart = null,
                                         int             duration            = 0,
                                         int             maxTeamPlayers      = 5,
                                         string?         organizationName    = "-",
                                         bool            isMixedGroups       = true)
    {
        tournamentDateStart ??= DateOnly.FromDateTime(DateTime.UtcNow);
        tournamentTimeStart ??= TimeOnly.FromDateTime(DateTime.UtcNow);
        addedTournament = new Tournament
                          {
                              OrganizerId      = organiserId,
                              TournamentName   = tournamentName,
                              SystemId         = systemId,
                              KindId           = kindId,
                              ToursCount       = toursCount,
                              Place            = place,
                              DateStart        = (DateOnly)tournamentDateStart,
                              TimeStart        = (TimeOnly)tournamentTimeStart,
                              Duration         = duration,
                              MaxTeamPlayers   = maxTeamPlayers,
                              OrganizationName = organizationName,
                              IsMixedGroups    = isMixedGroups
                          };
        try
        {
            _context.Entry(addedTournament).State = EntityState.Detached;
            _context.Tournaments.Add(addedTournament);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception)
        {
            _context.Entry(addedTournament).State = EntityState.Detached;
            addedTournament                       = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddPlayer(out Player? addedPlayer, int tournamentId, int organiserId, string lastName,
                                     string      firstName,
                                     char        gender      = 'M',
                                     string      attribute   = "-",
                                     int         birthYear   = 2000,
                                     int         boardNumber = 1,
                                     int?        teamId      = null,
                                     int?        groupId     = null,
                                     bool        isActive    = true)
    {
        addedPlayer = new Player
                      {
                          TournamentId    = tournamentId,
                          OrganizerId     = organiserId,
                          PlayerLastName  = lastName,
                          PlayerFirstName = firstName,
                          Gender          = gender,
                          PlayerAttribute = attribute,
                          PlayerBirthYear = birthYear,
                          BoardNumber     = boardNumber,
                          TeamId          = teamId,
                          GroupId         = groupId,
                          IsActive        = isActive
                      };
        try
        {
            _context.Entry(addedPlayer).State = EntityState.Detached;
            _context.Players.Add(addedPlayer);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception)
        {
            _context.Entry(addedPlayer).State = EntityState.Detached;
            addedPlayer                       = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddTeam(out Team? addedTeam,       int    organiserId, int tournamentId, string name,
                                   bool      isActive = true, string attribute = "-")
    {
        addedTeam = new Team
                    {
                        OrganizerId   = organiserId,
                        TournamentId  = tournamentId,
                        TeamName      = name,
                        TeamAttribute = attribute,
                        IsActive      = isActive
                    };
        try
        {
            _context.Entry(addedTeam).State = EntityState.Detached;
            _context.Teams.Add(addedTeam);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception)
        {
            _context.Entry(addedTeam).State = EntityState.Detached;
            addedTeam                       = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddGroup(out Group? addedGroup, int organiserId, int tournamentId,
                                    string     name     = "1",
                                    string     identity = "1")
    {
        addedGroup = new Group
                     {
                         OrganizerId  = organiserId,
                         TournamentId = tournamentId,
                         GroupName    = name,
                         Identity     = identity
                     };
        try
        {
            _context.Entry(addedGroup).State = EntityState.Detached;
            _context.Groups.Add(addedGroup);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception)
        {
            _context.Entry(addedGroup).State = EntityState.Detached;
            addedGroup                       = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddGamePair(out Game? game, int whiteId, int blackId, int tournamentId, int organizerId,
                                       int tourNumber,
                                       int whitePointsResult = 0, int blackPointsResult = 0, bool isPlayed = false)
    {
        game = new Game
               {
                   WhiteId      = whiteId,
                   BlackId      = blackId,
                   TournamentId = tournamentId,
                   OrganizerId  = organizerId,
                   TourNumber   = tourNumber,
                   WhitePoints  = whitePointsResult,
                   BlackPoints  = blackPointsResult,
                   IsPlayed     = isPlayed
               };
        try
        {
            _context.Entry(game).State = EntityState.Detached;
            _context.Games.Add(game);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception)
        {
            _context.Entry(game).State = EntityState.Detached;
            game                       = null;
            return InsertResult.Fail;
        }
    }
}

public enum InsertResult
{
    Success,
    Fail
}
