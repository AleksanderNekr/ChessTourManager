using System.Collections.Generic;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.DataAccess.Queries.Get;

public interface IGetQueries
{
    public static IGetQueries CreateInstance(ChessTourContext context)
    {
        return new GetQueries(context);
    }

    /// <summary>
    ///     Получение пользователя по его ID.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    /// <param name="user">Выходной параметр – пользователь.</param>
    /// <returns>
    ///     Если пользователь найден, то он возвращается, результат – Success,
    ///     иначе – null, результат – UserNotFound.
    /// </returns>
    public GetResult TryGetUserById(int id, out User? user);

    /// <summary>
    ///     Получение пользователя по его логину и паролю.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <param name="user">Выходной параметр – пользователь.</param>
    /// <returns>
    ///     Если пользователь найден, то он возвращается, результат – Success,
    ///     иначе – null, результат – UserNotFound.
    /// </returns>
    public GetResult TryGetUserByLoginAndPass(string? login, string password, out User? user);

    /// <summary>
    ///     Получение турниров пользователя по его ID.
    /// </summary>
    /// <param name="organiserId">ID пользователя.</param>
    /// <param name="tournaments">Выходной параметр – список турниров.</param>
    /// <returns>
    ///     Если пользователь найден, то возвращается список его турниров, результат – Success,
    ///     иначе – пустой список, результат – UserNotFound.
    /// </returns>
    public GetResult TryGetTournaments(int organiserId, out IEnumerable<Tournament>? tournaments);

    public GetResult TryGetTournamentsWithTeamsAndPlayers(int organiserId, out IEnumerable<Tournament?>? tournaments);

    /// <summary>
    ///     Получение списка игроков в турнире пользователя.
    /// </summary>
    /// <param name="organiserId">ID пользователя.</param>
    /// <param name="tournamentId">ID турнира.</param>
    /// <param name="players">Выходной параметр – список игроков.</param>
    /// <returns>
    ///     Если пользователь не найден, возвращается пустой список игроков, результат – UserNotFound,
    ///     если пользователь не имеет турниров, то возвращается NoTournaments, если турнир с заданным ID не найден,
    ///     то TournamentNotFound, иначе – список игроков и результат – Success.
    /// </returns>
    public GetResult TryGetPlayers(int organiserId, int tournamentId, out IEnumerable<Player>? players);

    public GetResult TryGetPlayersWithTeamsAndGroups(int                      organiserId, int tournamentId,
                                                     out IEnumerable<Player>? players);

    /// <summary>
    ///     Получение списка команд в турнире пользователя.
    /// </summary>
    /// <param name="organiserId">ID пользователя.</param>
    /// <param name="tournamentId">ID турнира.</param>
    /// <param name="teams">Выходной параметр – список команд.</param>
    /// <returns>
    ///     Если пользователь не найден, возвращается пустой список команд, результат – UserNotFound,
    ///     если пользователь не имеет турниров, то возвращается NoTournaments, если турнир с заданным ID не найден,
    ///     то TournamentNotFound, иначе – список команд и результат – Success.
    /// </returns>
    public GetResult TryGetTeamsWithPlayers(int organiserId, int tournamentId, out IEnumerable<Team>? teams);


    /// <summary>
    ///     Получение списка групп в турнире пользователя.
    /// </summary>
    public GetResult TryGetGroups(int                     organizerId, int tournamentId,
                                  out IEnumerable<Group>? groups);


    /// <summary>
    ///     Получение списка игр тура в турнире пользователя.
    /// </summary>
    public GetResult TryGetGames(int organiserId, int tournamentId, out IEnumerable<Game>? games);

    /// <summary>
    ///     Получение видов турниров.
    /// </summary>
    /// <param name="kinds">Выходной параметр – список видов турниров.</param>
    /// <returns>Список видов турниров и результат – Success. </returns>
    public GetResult GetKinds(out IEnumerable<Kind>? kinds);

    /// <summary>
    ///     Получение списка систем турниров.
    /// </summary>
    /// <param name="systems">Выходной параметр – список систем турниров.</param>
    /// <returns>Список систем турниров и результат – Success. </returns>
    public GetResult GetSystems(out IEnumerable<Entities.System>? systems);
}
