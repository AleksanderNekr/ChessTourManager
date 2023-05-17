using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.DataAccess.Queries.Insert;

public interface IInsertQueries
{
    public static IInsertQueries CreateInstance(ChessTourContext context)
    {
        return new InsertQueries(context);
    }

    /// <summary>
    ///     Добавление пользователя.
    /// </summary>
    public InsertResult TryAddUser(out User? user, string lastName, string firstName, string email, string password,
                                   string    patronymic       = "-",
                                   int       tournamentsLimit = 50);

    /// <summary>
    ///     Добавление турнира пользователя по его ID.
    /// </summary>
    public InsertResult TryAddTournament(out Tournament? addedTournament, int organiserId, string? tournamentName,
                                         int             systemId,        int kindId,
                                         int             toursCount          = 7,
                                         string?         place               = "-",
                                         DateOnly?       tournamentDateStart = null,
                                         TimeOnly?       tournamentTimeStart = null,
                                         int             duration            = 0,
                                         int             maxTeamPlayers      = 5,
                                         string?         organizationName    = "-",
                                         bool            isMixedGroups       = true);

    /// <summary>
    ///     Добавление игрока в список в турнире пользователя.
    /// </summary>
    public InsertResult TryAddPlayer(out Player? addedPlayer, int tournamentId, int organiserId, string lastName,
                                     string      firstName,
                                     char        gender      = 'M',
                                     string      attribute   = "-",
                                     int         birthYear   = 2000,
                                     int         boardNumber = 1,
                                     int?        teamId      = null,
                                     int?        groupId     = null,
                                     bool        isActive    = true);

    /// <summary>
    ///     Добавление команды в список турнира пользователя.
    /// </summary>
    public InsertResult TryAddTeam(out Team? addedTeam, int organiserId, int tournamentId, string name,
                                   bool      isActive  = true,
                                   string    attribute = "-");

    /// <summary>
    ///     Добавление группы в турнир пользователя.
    /// </summary>
    public InsertResult TryAddGroup(out Group? addedTeam, int organiserId, int tournamentId,
                                    string     name     = "1",
                                    string     identity = "1");

    /// <summary>
    ///     Добавление пары игроков в список пар тура.
    /// </summary>
    public InsertResult TryAddGamePair(out Game? game, int whiteId, int blackId, int tournamentId, int organizerId,
                                       int       tourNumber,
                                       int       whitePointsResult = 0,
                                       int       blackPointsResult = 0,
                                       bool      isPlayed          = false);
}
