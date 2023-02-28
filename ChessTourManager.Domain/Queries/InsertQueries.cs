using System;
using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Helpers;

namespace ChessTourManager.Domain.Queries;

internal class InsertQueries : IInsertQueries
{
    public InsertResult TryAddUser(string lastName, string firstName, string email, string password,
                                   string patronymic       = "-",
                                   int    tournamentsLimit = 50)
    {
        try
        {
            var context = ChessTourContext.CreateInstance();
            context.Users.Add(new User
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
            context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddTournament(int       organiserId, string tournamentName, int systemId, int kindId,
                                         int       toursCount          = 7,
                                         string    place               = "-",
                                         DateOnly? tournamentDateStart = null,
                                         TimeOnly? tournamentTimeStart = null,
                                         int       duration            = 0,
                                         int       maxTeamPlayers      = 5,
                                         string    organizationName    = "-",
                                         bool      isMixedGroups       = true)
    {
        try
        {
            tournamentDateStart ??= DateOnly.FromDateTime(DateTime.UtcNow);

            tournamentTimeStart ??= TimeOnly.FromDateTime(DateTime.UtcNow);

            var context = ChessTourContext.CreateInstance();
            context.Tournaments.Add(new Tournament
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
                                    });
            context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка при создании турнира", MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddPlayer(int    tournamentId, int organiserId, string lastName, string firstName,
                                     char   gender      = 'M',
                                     string attribute   = "-",
                                     int    birthYear   = 1900,
                                     int    boardNumber = 1,
                                     int?   teamId      = null,
                                     int?   groupId     = null,
                                     bool   isActive    = true)
    {
        try
        {
            var context = ChessTourContext.CreateInstance();
            context.Players.Add(new Player
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
                                });
            context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка добавлении игрока", MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddTeam(int  organiserId, int tournamentId, string name, string attribute = "-",
                                   bool isActive = true)
    {
        try
        {
            var context = ChessTourContext.CreateInstance();
            context.Teams.Add(new Team
                              {
                                  OrganizerId   = organiserId,
                                  TournamentId  = tournamentId,
                                  TeamName      = name,
                                  TeamAttribute = attribute,
                                  IsActive      = isActive
                              });
            context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка при добавлении команды", MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddGroup(int    organiserId, int tournamentId,
                                    string name     = "1",
                                    string identity = "1")
    {
        try
        {
            var context = ChessTourContext.CreateInstance();
            context.Groups.Add(new Group
                               {
                                   OrganizerId  = organiserId,
                                   TournamentId = tournamentId,
                                   GroupName    = name,
                                   Identity     = identity
                               });
            context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка при добавлении группы", MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }

    public InsertResult TryAddGamePair(int whiteId, int blackId, int tournamentId, int organizerId, int tourNumber,
                                       int whitePointsResult = 0, int blackPointsResult = 0, bool isPlayed = false)
    {
        try
        {
            var context = ChessTourContext.CreateInstance();
            context.Games.Add(new Game()
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
            context.SaveChanges();
            return InsertResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка при добавлении пары", MessageBoxButton.OK, MessageBoxImage.Error);
            return InsertResult.Fail;
        }
    }
}

public enum InsertResult
{
    Success,
    Fail
}
