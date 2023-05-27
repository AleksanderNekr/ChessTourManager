using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

    private async Task LoadTournamentAsync()
    {
        this.ViewBag.Tournament = await this._context.Tournaments.FindAsync(_tournamentId, _userId)
                               ?? throw new NullReferenceException("Tournament not found");
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
    public async Task<IActionResult> Index(int id, int? organizerId)
    {
        if (id != 0)
        {
            _tournamentId = id;
        }

        _userId = organizerId ?? _userId;

        IQueryable<Team> teams = this._context.Teams
                                     .Include(t => t.Tournament)
                                     .Include(t => t.Players)
                                     .ThenInclude(p => p.Group)
                                     .Where(t => t.TournamentId == _tournamentId && t.OrganizerId == _userId);

        await this.LoadTournamentAsync();

        return this.View(await teams.ToListAsync());
    }

    /// <summary>
    /// GET: Teams/Details/5
    /// </summary>
    /// <param name="id">Team id</param>
    /// <returns>Details view</returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();

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
    public async Task<IActionResult> Create()
    {
        await this.LoadTournamentAsync();
        return this.View(new Team());
    }

    /// <summary>
    /// POST: Teams/Create
    /// </summary>
    /// <param name="team">Team to create</param>
    /// <returns>Index view</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("TeamId,OrganizerId,TournamentId,TeamName,TeamAttribute,IsActive")]
        Team team)
    {
        await this.LoadTournamentAsync();
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
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();

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
    public async Task<IActionResult> Edit(int id,
                                          [Bind("TeamId,OrganizerId,TournamentId,TeamName,TeamAttribute,IsActive")]
                                          Team team)
    {
        await this.LoadTournamentAsync();

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
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();

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
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await this.LoadTournamentAsync();
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
