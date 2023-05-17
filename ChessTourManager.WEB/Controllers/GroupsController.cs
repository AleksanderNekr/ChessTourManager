using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

    private async Task LoadTournamentAsync()
    {
        this.ViewBag.Tournament = await this._context.Tournaments.FindAsync(_tournamentId, _userId)
                               ?? throw new NullReferenceException("Tournament not found");
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
    public async Task<IActionResult> Index(int id)
    {
        if (id != 0)
        {
            _tournamentId = id;
        }

        _userId = this._context.Users.First(u => u.UserName == this.User.Identity.Name).Id;

        IQueryable<Group> groups = this._context.Groups
                                       .Include(t => t.Tournament)
                                       .Where(t => t.TournamentId == _tournamentId && t.OrganizerId == _userId);

        await this.LoadTournamentAsync();

        return this.View(await groups.ToListAsync());
    }

    /// <summary>
    /// GET: Groups/Details/5
    /// </summary>
    /// <param name="id">Group id</param>
    /// <returns>Details view</returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();

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
    public async Task<IActionResult> Create()
    {
        await this.LoadTournamentAsync();
        return this.View(new Group());
    }

    /// <summary>
    /// POST: Groups/Create
    /// </summary>
    /// <param name="group">Group to create</param>
    /// <returns>Index view</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,OrganizerId,TournamentId,GroupName,Identity")]
        Group group)
    {
        await this.LoadTournamentAsync();
        if (!this.ModelState.IsValid)
        {
            return this.View(group);
        }

        this._context.Add(group);
        await this._context.SaveChangesAsync();

        this.TempData["Success"] = $"Group {group.GroupName} was successfully created!";
        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
    }

    /// <summary>
    /// GET: Groups/Edit/5
    /// </summary>
    /// <param name="id">Group id</param>
    /// <returns>Edit view</returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();

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
    public async Task<IActionResult> Edit(int id,
                                          [Bind("Id,OrganizerId,TournamentId,GroupName,Identity")]
                                          Group group)
    {
        await this.LoadTournamentAsync();

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
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();

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
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await this.LoadTournamentAsync();
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
