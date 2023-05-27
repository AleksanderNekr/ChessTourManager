using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WEB.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ChessTourManager.WEB.IntegrationTests;

public class WebTests
{
    private readonly ChessTourContext _context = new();
    private readonly int              _orgId   = 3;
    private readonly int              _tourId  = 2;

    [Test]
    public async Task Test1()
    {
        // Get games.
        IGetQueries.CreateInstance(this._context)
                   .TryGetGames(this._orgId, this._tourId, out List<Game>? games);

        // Open tournaments view.
        TournamentsController tournamentsController = new(this._context);
        await tournamentsController.Index(this._orgId);

        // Open tournament.
        PlayersController playersController = new(this._context);
        await playersController.Index(this._tourId, this._orgId);

        GamesController gamesController = new(this._context);
        await gamesController.Index(this._tourId, null, this._orgId);

        int oldTourNumber = games?.Count > 0
                                ? games.Max(g => g.TourNumber)
                                : 0;

        // Start new tour.
        gamesController.Create();


        // Get new games.
        IGetQueries.CreateInstance(this._context)
                   .TryGetGames(this._orgId, this._tourId, out games);
        // Check result.
        Assert.AreEqual(oldTourNumber + 1, games?.Max(g => g.TourNumber));
    }

    [Test]
    public async Task Test2()
    {
        // Get games.
        IGetQueries.CreateInstance(this._context)
                   .TryGetGames(this._orgId, this._tourId, out List<Game>? games);

        // Open tournaments view.
        TournamentsController tournamentsController = new(this._context);
        await tournamentsController.Index(this._orgId);

        // Open tournament.
        PlayersController playersController = new(this._context);
        await playersController.Index(this._tourId, this._orgId);

        GamesController gamesController = new(this._context);
        await gamesController.Index(this._tourId, null, this._orgId);

        int oldTourNumber = games?.Count > 0
                                ? games.Max(g => g.TourNumber)
                                : 0;

        // Add new player.
        InsertResult insRes = IInsertQueries.CreateInstance(this._context)
                                            .TryAddPlayer(out Player? _, this._tourId, this._orgId,
                                                          "Test"   + DateTime.Now.Ticks,
                                                          "Player" + DateTime.Now.Ticks);
        Assert.AreEqual(InsertResult.Success, insRes);

        // Start new tour.
        gamesController.Create();

        // Get new games.
        IGetQueries.CreateInstance(this._context)
                   .TryGetGames(this._orgId, this._tourId, out games);
        // Check result.
        Assert.AreEqual(oldTourNumber + 1, games?.Max(g => g.TourNumber));
    }
}
