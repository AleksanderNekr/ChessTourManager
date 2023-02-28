using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Queries;

public interface IDeleteQueries
{
    public static IDeleteQueries CreateInstance(ChessTourContext? context) => new DeleteQuery(context);

    public        DeleteResult   TryDeletePlayer(Player           player);
}

public enum DeleteResult
{
    Success,
    Failed
}
