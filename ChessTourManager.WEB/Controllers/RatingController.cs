using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WEB.Controllers;

/// <inheritdoc />
/// <summary>
///     The controller for the players of a tournament.
/// </summary>
public class RatingController : Controller
{
    private static   int              _tournamentId;
    private static   int              _userId;
    private readonly ChessTourContext _context;

    /// <summary>
    ///     The constructor of the controller.
    /// </summary>
    /// <param name="context">The context of the database.</param>
    public RatingController(ChessTourContext context)
    {
        this._context = context;
    }

    /// <summary>
    ///     GET: Players/Index.
    /// </summary>
    /// <returns>Index view.</returns>
    public async Task<IActionResult> Index(int id, int? organiserId)
    {
        if (id != 0)
        {
            _tournamentId = id;
        }

        _userId = organiserId ?? _userId;
        await this.LoadTournamentAsync();
        List<Player> players = await this._context.Players
                                         .Where(player => player.TournamentId == id)
                                         .Include(player => player.Team)
                                         .Include(player => player.Group)
                                         .OrderByDescending(player => player.PointsAmount)
                                         .ThenByDescending(player => player.RatioSum1)
                                         .ThenByDescending(player => player.RatioSum2)
                                         .ToListAsync();
        return this.View(players);
    }

    private async Task LoadTournamentAsync()
    {
        this.ViewBag.Tournament = await this._context.Tournaments.FindAsync(_tournamentId, _userId)
                               ?? throw new NullReferenceException("Tournament not found");
    }
}
