using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WEB.Controllers;

/// <inheritdoc />
/// <summary>
/// The controller for the tournaments.
/// </summary>
public class TournamentsController : Controller
{
    private readonly ChessTourContext _context;
    private static   int?             _organizerId;

    /// <summary>
    /// The constructor of the controller.
    /// </summary>
    /// <param name="context">The context of the database.</param>
    public TournamentsController(ChessTourContext context)
    {
        this._context = context;
    }

    private async Task UpdateOrganizerId()
    {
        if (this.User.Identity == null)
        {
            return;
        }

        User organizer = await this._context.Users
                                   .FirstAsync(u => u.UserName == this.User.Identity.Name);
        _organizerId = organizer.Id;
    }

    /// <summary>
    /// GET: Tournaments.
    /// </summary>
    /// <returns>The view of the tournaments.</returns>
    [Authorize]
    public async Task<IActionResult> Index()
    {
        await this.UpdateOrganizerId();
        await this.LoadKindsToViewBagAsync();
        await this.LoadSystemsToViewBagAsync();

        return this.View(await this._context.Tournaments.ToListAsync());
    }

    /// <summary>
    /// GET: Tournaments/Create.
    /// </summary>
    /// <returns>The create view of the tournament.</returns>
    [Authorize]
    public async Task<IActionResult> Create()
    {
        await this.LoadSystemsToViewBagAsync();
        await this.LoadKindsToViewBagAsync();

        return this.View(new Tournament
                         {
                             OrganizerId = _organizerId
                                        ?? throw new InvalidOperationException("OrganizerId is null")
                         });
    }

    /// <summary>
    /// POST: Tournaments/Create.
    /// </summary>
    /// <param name="tournament">The tournament.</param>
    /// <returns>The index view of the tournaments.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(Tournament tournament)
    {
        await this.LoadSystemsToViewBagAsync();
        await this.LoadKindsToViewBagAsync();

        if (!this.ModelState.IsValid)
        {
            return this.View(tournament);
        }

        await this._context.Tournaments.AddAsync(tournament);
        await this._context.SaveChangesAsync();

        this.TempData["Success"] = $"Tournament {tournament.TournamentName} created successfully!";
        return this.RedirectToAction(nameof(this.Index));
    }

    /// <summary>
    /// GET: Tournaments/Edit/{id}.
    /// </summary>
    /// <param name="id">The id of the tournament.</param>
    /// <returns>The edit view of the tournament.</returns>
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        await this.LoadKindsToViewBagAsync();
        await this.LoadSystemsToViewBagAsync();

        IGetQueries.CreateInstance(this._context)
                   .TryGetTournaments((int)_organizerId, out List<Tournament>? tournaments);
        Tournament? tournament = tournaments?.Find(t => t.Id == id && t.OrganizerId == _organizerId);

        if (tournament == null)
        {
            return this.NotFound();
        }

        return this.View(tournament);
    }

    /// <summary>
    /// POST: Tournaments/Edit/{id}.
    /// </summary>
    /// <param name="tournament">The tournament.</param>
    /// <returns>The index view of the tournaments.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(Tournament tournament)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(tournament);
        }

        try
        {
            tournament.DateLastChange = DateOnly.FromDateTime(DateTime.Now);
            tournament.TimeLastChange = TimeOnly.FromDateTime(DateTime.Now);
            this._context.Tournaments.Update(tournament);
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await this.TournamentExists(tournament.Id))
            {
                return this.NotFound();
            }

            throw;
        }

        this.TempData["Success"] = $"Tournament {tournament.TournamentName} edited successfully!";
        return this.RedirectToAction(nameof(this.Index));
    }

    private async Task<bool> TournamentExists(int tournamentId)
    {
        return await this._context.Tournaments.AnyAsync(tournament =>
                                                            tournament             != null
                                                         && tournament.Id          == tournamentId
                                                         && tournament.OrganizerId == _organizerId);
    }

    /// <summary>
    /// GET: Tournaments/Details/{id}.
    /// </summary>
    /// <param name="id">The id of the tournament.</param>
    /// <returns>The details view of the tournament.</returns>
    public async Task<IActionResult> Details(int id)
    {
        await this.LoadKindsToViewBagAsync();
        await this.LoadSystemsToViewBagAsync();

        IGetQueries.CreateInstance(this._context)
                   .TryGetTournaments((int)_organizerId, out List<Tournament>? tournaments);
        Tournament? tournament = tournaments?.Find(t => t.Id == id && t.OrganizerId == _organizerId);

        if (tournament == null)
        {
            return this.NotFound();
        }

        return this.View(tournament);
    }

    /// <summary>
    /// GET: Tournaments/Delete/{id}.
    /// </summary>
    /// <param name="id">The id of the tournament.</param>
    /// <returns>The delete view of the tournament.</returns>
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        await this.LoadKindsToViewBagAsync();
        await this.LoadSystemsToViewBagAsync();

        IGetQueries.CreateInstance(this._context)
                   .TryGetTournaments((int)_organizerId, out List<Tournament>? tournaments);
        Tournament? tournament = tournaments?.Find(t => t.Id == id && t.OrganizerId == _organizerId);

        if (tournament == null)
        {
            return this.NotFound();
        }

        return this.View(tournament);
    }

    /// <summary>
    /// POST: Tournaments/Delete/{id}.
    /// </summary>
    /// <param name="id">The id of the tournament.</param>
    /// <returns>The index view of the tournaments.</returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournaments((int)_organizerId, out List<Tournament>? tournaments);
        Tournament? tournament = tournaments?.Find(t => t.Id == id && t.OrganizerId == _organizerId);

        if (tournament == null)
        {
            return this.RedirectToAction(nameof(this.Index));
        }

        this._context.Tournaments.Remove(tournament);
        await this._context.SaveChangesAsync();

        this.TempData["Success"] = $"Tournament {tournament.TournamentName} deleted successfully!";
        return this.RedirectToAction(nameof(this.Index));
    }

    private async Task LoadSystemsToViewBagAsync()
    {
        this.ViewBag.Systems = await this._context.Systems.Select(s => new SelectListItem
                                                                       {
                                                                           Text  = s.SystemNameLocalized,
                                                                           Value = s.Id.ToString(),
                                                                       }).ToListAsync();
    }

    private async Task LoadKindsToViewBagAsync()
    {
        this.ViewBag.Kinds = await this._context.Kinds.Select(s => new SelectListItem
                                                                   {
                                                                       Text  = s.KindNameLocalized,
                                                                       Value = s.Id.ToString(),
                                                                   }).ToListAsync();
    }
}
