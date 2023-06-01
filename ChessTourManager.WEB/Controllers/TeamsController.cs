using System.Security.Claims;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WEB.Controllers;

/// <inheritdoc />
/// <summary>
/// Controller for teams.
/// </summary>
public class TeamsController : Controller
{
    private readonly ChessTourContext _context;
    private static   int              _tournamentId;
    private static   int              _userId;

    private async Task<IActionResult> LoadTournamentAsync()
    {
        Tournament? tournament = await this._context.Tournaments.FindAsync(_tournamentId, _userId);
        if (tournament is null)
        {
            this.TempData["Error"] = "Tournament not found";
            return this.RedirectToAction("Index", "Tournaments");
        }

        this.ViewBag.Tournament = tournament;
        return this.Ok();
    }

    /// <summary>
    /// Constructor for TeamsController.
    /// </summary>
    /// <param name="context">ChessTourContext.</param>
    public TeamsController(ChessTourContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// GET: Teams
    /// </summary>
    /// <returns>Index view</returns>
    [Authorize]
    public async Task<IActionResult> Index(int id, int? organizerId)
    {
        if (id != 0)
        {
            _tournamentId = id;
        }

        if (this.User.Identity?.IsAuthenticated ?? false)
        {
            _userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) is null
                          ? 0
                          : int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)
                                   ?? throw new InvalidOperationException("User ID is null"));
        }
        else
        {
            _userId = organizerId ?? _userId;
        }

        IQueryable<Team> teams = this._context.Teams
                                     .Include(t => t.Tournament)
                                     .Include(t => t.Players)
                                     .ThenInclude(p => p.Group)
                                     .Where(t => t.TournamentId == _tournamentId && t.OrganizerId == _userId);

        IActionResult res = await this.LoadTournamentAsync();
        if (res is not OkResult)
        {
            return res;
        }

        return this.View(await teams.ToListAsync());
    }

    /// <summary>
    /// GET: Teams/Details/5
    /// </summary>
    /// <param name="id">Team id</param>
    /// <returns>Details view</returns>
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        Team? team = await this._context.Teams
                               .Include(t => t.Tournament)
                               .FirstOrDefaultAsync(m => m.Id           == id
                                                      && m.TournamentId == _tournamentId
                                                      && m.OrganizerId  == _userId);
        if (team == null)
        {
            return this.NotFound();
        }

        return this.View(team);
    }

    /// <summary>
    /// GET: Teams/Create
    /// </summary>
    /// <returns>Create view</returns>
    [Authorize]
    public async Task<IActionResult> Create()
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        return this.View(new Team());
    }

    /// <summary>
    /// POST: Teams/Create
    /// </summary>
    /// <param name="team">Team to create</param>
    /// <returns>Index view</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(
        [Bind("TeamId,OrganizerId,TournamentId,TeamName,TeamAttribute,IsActive")]
        Team team)
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        if (!this.ModelState.IsValid)
        {
            return this.View(team);
        }

        this._context.Add(team);
        await this._context.SaveChangesAsync();

        if (this.TempData != null)
        {
            this.TempData["Success"] = $"Team {team.TeamName} was successfully created!";
        }

        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
    }

    /// <summary>
    /// GET: Teams/Edit/5
    /// </summary>
    /// <param name="id">Team id</param>
    /// <returns>Edit view</returns>
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        Team? team = await this._context.Teams.FindAsync(id, _userId, _tournamentId);
        if (team == null)
        {
            return this.NotFound();
        }

        return this.View(team);
    }

    /// <summary>
    /// POST: Teams/Edit/5
    /// </summary>
    /// <param name="id">Id of team to edit</param>
    /// <param name="team">Team to edit</param>
    /// <returns>Index view</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int id,
                                          [Bind("TeamId,OrganizerId,TournamentId,TeamName,TeamAttribute,IsActive")]
                                          Team team)
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        if (!this.ModelState.IsValid)
        {
            return this.View(team);
        }

        try
        {
            team.Id = id;
            this._context.Teams.Update(team);
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.TeamExists(team.Id))
            {
                return this.NotFound();
            }

            throw;
        }

        this.TempData["Success"] = $"Team {team.TeamName} was successfully edited!";
        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
    }

    /// <summary>
    /// GET: Teams/Delete/5
    /// </summary>
    /// <param name="id">Team id</param>
    /// <returns>Delete view</returns>
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        Team? team = await this._context.Teams
                               .Include(t => t.Tournament)
                               .FirstOrDefaultAsync(m => m.Id           == id
                                                      && m.TournamentId == _tournamentId
                                                      && m.OrganizerId  == _userId);
        if (team == null)
        {
            return this.NotFound();
        }

        return this.View(team);
    }

    /// <summary>
    /// POST: Teams/Delete/5
    /// </summary>
    /// <param name="id">Team id</param>
    /// <returns>Index view</returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        Team? team = await this._context.Teams.FindAsync(id, _userId, _tournamentId);
        if (team != null)
        {
            this._context.Teams.Remove(team);
        }

        await this._context.SaveChangesAsync();

        this.TempData["Success"] = $"Team {team?.TeamName} was successfully deleted!";
        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
    }

    private bool TeamExists(int id)
    {
        return this._context.Teams.Any(e => e.Id           == id
                                         && e.TournamentId == _tournamentId
                                         && e.OrganizerId  == _userId);
    }
}
