using System.Security.Claims;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WEB.Controllers;

/// <inheritdoc />
/// <summary>
///     The controller for the players of a tournament.
/// </summary>
public class PlayersController : Controller
{
    private static   int              _tournamentId;
    private static   int              _userId;
    private readonly ChessTourContext _context;

    /// <summary>
    ///     The constructor of the controller.
    /// </summary>
    /// <param name="context">The context of the database.</param>
    public PlayersController(ChessTourContext context)
    {
        this._context = context;
    }

    /// <summary>
    ///     GET: Players/Index.
    /// </summary>
    /// <returns>Index view.</returns>
    [HttpGet("/Players/Index/{id:int?}")]
    [Authorize]
    public async Task<IActionResult> Index(int id, int? organiserId)
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
            _userId = organiserId ?? _userId;
        }

        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();
        List<Player> players = await this._context.Players
                                         .Where(player => player.TournamentId == id && player.OrganizerId == _userId)
                                         .Include(player => player.Team)
                                         .Include(player => player.Group)
                                         .ToListAsync();
        return this.View(players);
    }

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

    private async Task LoadTeamsAsync()
    {
        this.ViewBag.Teams = await this._context.Teams
                                       .Where(team => team.TournamentId == _tournamentId)
                                       .Select(s => new SelectListItem
                                                    {
                                                        Text  = s.TeamName,
                                                        Value = s.Id.ToString()
                                                    })
                                       .ToListAsync();
    }

    private async Task LoadGroupsAsync()
    {
        this.ViewBag.Groups = await this._context.Groups
                                        .Where(group => group.TournamentId == _tournamentId)
                                        .Select(s => new SelectListItem
                                                     {
                                                         Text  = s.GroupName,
                                                         Value = s.Id.ToString()
                                                     })
                                        .ToListAsync();
    }

    /// <summary>
    ///     GET: Players/Create.
    /// </summary>
    [Authorize]
    public async Task<IActionResult> Create()
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();
        return this.View(new Player());
    }

    /// <summary>
    ///     POST: Players/Create.
    /// </summary>
    /// <param name="player">The player to create.</param>
    /// <returns>Index view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(
        [Bind($"{
            nameof(Player.Id)
        },{
            nameof(Player.TournamentId)
        },{
            nameof(Player.OrganizerId)
        },{
            nameof(Player.TeamId)
        },{
            nameof(Player.GroupId)
        },{
            nameof(Player.IsActive)
        },{
            nameof(Player.PlayerLastName)
        },{
            nameof(Player.PlayerFirstName)
        },{
            nameof(Player.PlayerFullName)
        },{
            nameof(Player.Gender)
        },{
            nameof(Player.PlayerAttribute)
        },{
            nameof(Player.PlayerBirthYear)
        },{
            nameof(Player.BoardNumber)
        },{
            nameof(Player.WinsCount)
        },{
            nameof(Player.DrawsCount)
        },{
            nameof(Player.LossesCount)
        },{
            nameof(Player.PointsAmount)
        },{
            nameof(Player.RatioSum1)
        },{
            nameof(Player.RatioSum2)
        }")]
        Player player)
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();

        if (!this.ModelState.IsValid)
        {
            return this.View(player);
        }

        this._context.Players.Add(player);
        await this._context.SaveChangesAsync();
        if (this.TempData != null)
        {
            this.TempData["Success"] = $"Player {player.PlayerFullName} created successfully!";
        }

        return this.RedirectToAction(nameof(this.Index), new { id = player.TournamentId });
    }

    /// <summary>
    ///     GET: Players/Edit.
    /// </summary>
    /// <param name="id">The id of the player to edit.</param>
    /// <returns>Edit player view.</returns>
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

        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();

        Player? player = await this._context.Players.FindAsync(id, _tournamentId, _userId);
        if (player == null)
        {
            return this.NotFound();
        }

        return this.View(player);
    }

    /// <summary>
    ///     POST: Players/Edit.
    /// </summary>
    /// <param name="player">The player to edit.</param>
    /// <returns>Index view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(
        [Bind($"{
            nameof(Player.Id)
        },{
            nameof(Player.TournamentId)
        },{
            nameof(Player.OrganizerId)
        },{
            nameof(Player.TeamId)
        },{
            nameof(Player.GroupId)
        },{
            nameof(Player.IsActive)
        },{
            nameof(Player.PlayerLastName)
        },{
            nameof(Player.PlayerFirstName)
        },{
            nameof(Player.PlayerFullName)
        },{
            nameof(Player.Gender)
        },{
            nameof(Player.PlayerAttribute)
        },{
            nameof(Player.PlayerBirthYear)
        },{
            nameof(Player.BoardNumber)
        }")]
        Player player)
    {
        if (await this.LoadTournamentAsync() is not OkResult)
        {
            return this.RedirectToAction("Index", "Tournaments");
        }

        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();

        if (!this.ModelState.IsValid)
        {
            return this.View(player);
        }

        this._context.Players.Update(player);
        await this._context.SaveChangesAsync();

        this.TempData["Success"] = $"Player {player.PlayerFullName} updated successfully!";
        return this.RedirectToAction(nameof(this.Index), new { id = player.TournamentId });
    }

    /// <summary>
    ///     GET: Players/Delete.
    /// </summary>
    /// <param name="id">The id of the player to delete.</param>
    /// <returns>Delete player view.</returns>
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

        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();


        Player player = this._context.Players
                            .Include(player => player.Team)
                            .Include(player => player.Group)
                            .Single(p => p.Id           == id
                                      && p.TournamentId == _tournamentId
                                      && p.OrganizerId  == _userId);

        return this.View(player);
    }

    /// <summary>
    ///     POST: Players/Delete.
    /// </summary>
    /// <param name="id">The id of the player to delete.</param>
    /// <returns>Index view.</returns>
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

        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();

        Player? player = await this._context.Players.FindAsync(id, _tournamentId, _userId);
        if (player == null)
        {
            return this.NotFound();
        }

        DeleteResult res = IDeleteQueries.CreateInstance(this._context)
                                         .TryDeletePlayer(player);
        if (res == DeleteResult.Success)
        {
            this.TempData["Success"] = $"Player {player.PlayerFullName} deleted successfully!";
        }
        else
        {
            this.TempData["Error"] = $"Player {player.PlayerFullName} could not be deleted!";
        }

        return this.RedirectToAction(nameof(this.Index), new { id = player.TournamentId });
    }

    /// <summary>
    ///     GET: Players/Details.
    /// </summary>
    /// <param name="id">The id of the player to show.</param>
    /// <returns>Details view.</returns>
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

        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();

        Player player = this._context.Players
                            .Include(player => player.Team)
                            .Include(player => player.Group)
                            .Single(p => p.Id           == id
                                      && p.TournamentId == _tournamentId
                                      && p.OrganizerId  == _userId);

        return this.View(player);
    }
}
