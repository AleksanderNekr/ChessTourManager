using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Queries;

public interface IGetQueries
{
    public static IGetQueries CreateInstance(ChessTourContext context) => new GetQueries(context);

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
    /// <param name="user">Выходной параметр – пользователь.</param>
    /// <returns>
    ///     Если пользователь найден, то он возвращается, результат – Success,
    ///     иначе – null, результат – UserNotFound.
    /// </returns>
    public GetResult TryGetUserByLoginAndPass(string login, string password, out User? user);

    /// <summary>
    ///     Получение турниров пользователя по его ID.
    /// </summary>
    /// <param name="organiserId">ID пользователя.</param>
    /// <param name="tournaments">Выходной параметр – список турниров.</param>
    /// <returns>
    ///     Если пользователь найден, то возвращается список его турниров, результат – Success,
    ///     иначе – пустой список, результат – UserNotFound.
    /// </returns>
    public GetResult TryGetTournaments(int organiserId, out IQueryable<Tournament>? tournaments);

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
    public GetResult TryGetPlayers(int organiserId, int tournamentId, out IQueryable<Player>? players);

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
    public GetResult TryGetTeams(int organiserId, int tournamentId, out IQueryable<Team>? teams);


    /// <summary>
    ///     Получение списка групп в турнире пользователя.
    /// </summary>
    public GetResult TryGetGroups(int organiserId, int tournamentId, out IQueryable<Group>? groups);


    /// <summary>
    ///     Получение списка игр тура в турнире пользователя.
    /// </summary>
    public GetResult TryGetGames(int organiserId, int tournamentId, int tourNumber, out IQueryable<Game>? games);
}
