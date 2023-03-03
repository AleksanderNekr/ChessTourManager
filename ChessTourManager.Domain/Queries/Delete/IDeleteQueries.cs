using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Queries.Delete;

public interface IDeleteQueries
{
    public static IDeleteQueries CreateInstance(ChessTourContext? context) => new DeleteQuery(context);

    public DeleteResult TryDeletePlayer(Player player);

    public DeleteResult TryDeleteTournament(Tournament tournament);
}

public enum DeleteResult
{
    Success,
    Failed
}
