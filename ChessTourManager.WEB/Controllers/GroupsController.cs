using System.Security.Claims;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WEB.Controllers;

/// <inheritdoc />
/// <summary>
/// Controller for groups.
/// </summary>
public class GroupsController : Controller
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
    /// Constructor for GroupsController.
    /// </summary>
    /// <param name="context">ChessTourContext.</param>
    public GroupsController(ChessTourContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// GET: Groups
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

        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        IQueryable<Group> groups = this._context.Groups
                                       .Include(t => t.Tournament)
                                       .Include(t => t.Players)
                                       .ThenInclude(p => p.Team)
                                       .Where(t => t.TournamentId == _tournamentId && t.OrganizerId == _userId);

        return this.View(await groups.ToListAsync());
    }

    /// <summary>
    /// GET: Groups/Details/5
    /// </summary>
    /// <param name="id">Group id</param>
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

        Group? group = await this._context.Groups
                                 .Include(t => t.Tournament)
                                 .FirstOrDefaultAsync(m => m.Id           == id
                                                        && m.TournamentId == _tournamentId
                                                        && m.OrganizerId  == _userId);
        if (group == null)
        {
            return this.NotFound();
        }

        return this.View(group);
    }

    /// <summary>
    /// GET: Groups/Create
    /// </summary>
    /// <returns>Create view</returns>
    [Authorize]
    public async Task<IActionResult> Create()
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        return this.View(new Group());
    }

    /// <summary>
    /// POST: Groups/Create
    /// </summary>
    /// <param name="group">Group to create</param>
    /// <returns>Index view</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(
        [Bind("Id,OrganizerId,TournamentId,GroupName,Identity")]
        Group group)
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        if (!this.ModelState.IsValid)
        {
            return this.View(group);
        }

        this._context.Add(group);
        await this._context.SaveChangesAsync();

        if (this.TempData != null)
        {
            this.TempData["Success"] = $"Group {group.GroupName} was successfully created!";
        }

        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
    }

    /// <summary>
    /// GET: Groups/Edit/5
    /// </summary>
    /// <param name="id">Group id</param>
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

        Group? group = await this._context.Groups.FindAsync(id, _tournamentId, _userId);
        if (group == null)
        {
            return this.NotFound();
        }

        return this.View(group);
    }

    /// <summary>
    /// POST: Groups/Edit/5
    /// </summary>
    /// <param name="id">Id of group to edit</param>
    /// <param name="group">Group to edit</param>
    /// <returns>Index view</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int id,
                                          [Bind("Id,OrganizerId,TournamentId,GroupName,Identity")]
                                          Group group)
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        if (!this.ModelState.IsValid)
        {
            return this.View(group);
        }

        try
        {
            group.Id = id;
            this._context.Groups.Update(group);
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.GroupExists(group.Id))
            {
                return this.NotFound();
            }

            throw;
        }

        this.TempData["Success"] = $"Group {group.GroupName} was successfully edited!";
        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
    }

    /// <summary>
    /// GET: Groups/Delete/5
    /// </summary>
    /// <param name="id">Group id</param>
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

        Group? group = await this._context.Groups
                                 .Include(t => t.Tournament)
                                 .FirstOrDefaultAsync(m => m.Id           == id
                                                        && m.TournamentId == _tournamentId
                                                        && m.OrganizerId  == _userId);
        if (group == null)
        {
            return this.NotFound();
        }

        return this.View(group);
    }

    /// <summary>
    /// POST: Groups/Delete/5
    /// </summary>
    /// <param name="id">Group id</param>
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

        Group? group = await this._context.Groups.FindAsync(id, _tournamentId, _userId);
        if (group != null)
        {
            this._context.Groups.Remove(group);
        }

        await this._context.SaveChangesAsync();

        this.TempData["Success"] = $"Group {group?.GroupName} was successfully deleted!";
        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
    }

    private bool GroupExists(int id)
    {
        return this._context.Groups.Any(e => e.Id           == id
                                          && e.TournamentId == _tournamentId
                                          && e.OrganizerId  == _userId);
    }
}
