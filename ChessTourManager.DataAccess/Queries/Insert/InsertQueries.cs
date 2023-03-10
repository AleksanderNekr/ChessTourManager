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
        catch (DbUpdateException)
        {
            MessageBox.Show("Ошибка в веденных данных! Возможно пользователь с таким email уже существует!",
                            "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
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
        catch (DbUpdateException)
        {
            MessageBox.Show("Ошибка в веденных данных! Возможно игрок с такими данными уже существует,"
                          + " либо вы не заполнили важные данные", "Ошибка добавлении игрока", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            addedPlayer = null;
            return InsertResult.Fail;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка добавлении игрока", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            addedPlayer = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddTeam(out Team? addedTeam,       int    organiserId, int tournamentId, string name,
                                   bool      isActive = true, string attribute = "-")
    {
        try
        {
            addedTeam = new Team
                        {
                            OrganizerId   = organiserId,
                            TournamentId  = tournamentId,
                            TeamName      = name,
                            TeamAttribute = attribute,
                            IsActive      = isActive
                        };
            _context.Teams.Add(addedTeam);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Ошибка в веденных данных! Возможно команда с таким именем уже существует,"
                          + " либо вы не заполнили важные данные", "Ошибка при добавлении команды",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            addedTeam = null;
            return InsertResult.Fail;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при добавлении команды",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            addedTeam = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddGroup(out Group? addedGroup, int organiserId, int tournamentId,
                                    string     name     = "1",
                                    string     identity = "1")
    {
        try
        {
            addedGroup = new Group
                         {
                             OrganizerId  = organiserId,
                             TournamentId = tournamentId,
                             GroupName    = name,
                             Identity     = identity
                         };
            _context.Groups.Add(addedGroup);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Ошибка в веденных данных! Возможно группа с таким именем уже существует,"
                          + " либо вы не заполнили важные данные", "Ошибка при добавлении группы",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            addedGroup = null;
            return InsertResult.Fail;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при добавлении группы",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            addedGroup = null;
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddGamePair(out Game? game, int whiteId, int blackId, int tournamentId, int organizerId,
                                       int tourNumber,
                                       int whitePointsResult = 0, int blackPointsResult = 0, bool isPlayed = false)
    {
        try
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
            _context.Games.Add(game);
            _context.SaveChanges();
            return InsertResult.Success;
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Ошибка в веденных данных! Возможно пара с такими данными уже существует,"
                          + " либо вы не заполнили важные данные", "Ошибка при добавлении пары",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            game = null;
            return InsertResult.Fail;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при добавлении пары",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            game = null;
            return InsertResult.Fail;
        }
    }
}

public enum InsertResult
{
    Success,
    Fail
}
