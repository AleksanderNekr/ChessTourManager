using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Queries.Delete;

public interface IDeleteQueries
{
    public static IDeleteQueries CreateInstance(ChessTourContext? context)
    {
        if (context != null)
        {
            return new DeleteQuery(context);
        }

        return new DeleteQuery(new ChessTourContext());
    }

    public DeleteResult TryDeletePlayer(Player player);

    public DeleteResult TryDeleteTournament(Tournament tournament);
}

public enum DeleteResult
{
    Success,
    Failed
}
