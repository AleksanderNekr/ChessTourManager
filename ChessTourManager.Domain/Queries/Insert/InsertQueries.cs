using System;
using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.Domain.Queries.Insert;

internal class InsertQueries : IInsertQueries
{
    private static ChessTourContext _context = new();

    public InsertQueries(ChessTourContext context)
    {
        _context = context;
    }

    public InsertResult TryAddUser(string lastName, string firstName, string email, string password,
                                   string patronymic       = "-",
                                   int    tournamentsLimit = 50)
    {
        try
        {
            _context.Users.Add(new User
                               {
                                   UserLastname   = lastName,
                                   UserFirstname  = firstName,
                                   Email          = email,
                                   PassHash       = PasswordHasher.HashPassword(password),
                                   UserPatronymic = patronymic,
                                   TournamentsLim = tournamentsLimit,
                                   RegisterDate   = DateOnly.FromDateTime(DateTime.UtcNow),
                                   RegisterTime   = TimeOnly.FromDateTime(DateTime.UtcNow)
                               });
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при регистрации",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddTournament(
        out Tournament? addedTournament, int organiserId, string tournamentName, int systemId, int kindId,
        int             toursCount          = 7,
        string          place               = "-",
        DateOnly?       tournamentDateStart = null,
        TimeOnly?       tournamentTimeStart = null,
        int             duration            = 0,
        int             maxTeamPlayers      = 5,
        string          organizationName    = "-",
        bool            isMixedGroups       = true)
    {
        try
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

            _context.Tournaments.Add(addedTournament);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Ошибка в веденных данных! Возможно турнир с таким именем уже существует,"
                          + " либо вы не заполнили важные данные", "Ошибка при создании турнира",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            addedTournament = null;
            return InsertResult.Fail;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при создании турнира",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            addedTournament = null;
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
        try
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

            _context.Players.Add(addedPlayer);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка добавлении игрока", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            addedPlayer = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddTeam(int  organiserId, int tournamentId, string name, string attribute = "-",
                                   bool isActive = true)
    {
        try
        {
            _context.Teams.Add(new Team
                               {
                                   OrganizerId   = organiserId,
                                   TournamentId  = tournamentId,
                                   TeamName      = name,
                                   TeamAttribute = attribute,
                                   IsActive      = isActive
                               });
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при добавлении команды",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddGroup(int    organiserId, int tournamentId,
                                    string name     = "1",
                                    string identity = "1")
    {
        try
        {
            _context.Groups.Add(new Group
                                {
                                    OrganizerId  = organiserId,
                                    TournamentId = tournamentId,
                                    GroupName    = name,
                                    Identity     = identity
                                });
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при добавлении группы",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddGamePair(int whiteId, int blackId, int tournamentId, int organizerId, int tourNumber,
                                       int whitePointsResult = 0, int blackPointsResult = 0, bool isPlayed = false)
    {
        try
        {
            _context.Games.Add(new Game
                               {
                                   WhiteId      = whiteId,
                                   BlackId      = blackId,
                                   TournamentId = tournamentId,
                                   OrganizerId  = organizerId,
                                   TourNumber   = tourNumber,
                                   WhitePoints  = whitePointsResult,
                                   BlackPoints  = blackPointsResult,
                                   IsPlayed     = isPlayed
                               });
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при добавлении пары",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }
}

public enum InsertResult
{
    Success,
    Fail
}
