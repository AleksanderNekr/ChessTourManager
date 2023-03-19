using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Delete;
using ChessTourManager.DataAccess.Queries.Get;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.UnitTests;

[TestFixture]
public class DeleteQueriesTests
{
    // Test on players:
    // 1.	Incorrect deletion (impossible to delete player that played in games).
    // 2.	Correct deletion.

    // 1. Incorrect deletion (impossible to delete player that played in games).
    [Test]
    public void TryDeletePlayer_WhenPlayerPlayedInGames_ReturnsFail()
    {
        // Arrange.
        var context = new ChessTourContext();
        var queries = IDeleteQueries.CreateInstance(context);
        Player? player = context.Games
                                .Include(g => g.PlayerWhite)
                                .First()?.PlayerWhite;
        // Act.
        DeleteResult result = queries.TryDeletePlayer(player);

        // Assert.
        Assert.AreEqual(DeleteResult.Failed, result);
    }

    // 2. Correct deletion.
    [Test]
    public void TryDeletePlayer_Correct()
    {
        // Arrange.
        var context = new ChessTourContext();
        var queries = IDeleteQueries.CreateInstance(context);

        // Get player that not take part in any games.
        // ReSharper disable once ComplexConditionExpression
        Player? player = context.Players
                                .First(p => !context.Games
                                                    .Include(g => g.PlayerWhite)
                                                    .Include(g => g.PlayerBlack)
                                                    .Any(g => g.PlayerWhite.PlayerId == p.PlayerId
                                                           || g.PlayerBlack.PlayerId == p.PlayerId));

        // Act.
        DeleteResult result = queries.TryDeletePlayer(player);

        // Assert.
        Assert.AreEqual(DeleteResult.Success, result);
    }
}
