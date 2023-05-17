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
    public async Task<IActionResult> Index(int id)
    {
        if (id != 0)
        {
            _tournamentId = id;
        }

        _userId = this._context.Users.First(u => u.UserName == this.User.Identity.Name).Id;
        await this.LoadTournamentAsync();
        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();
        List<Player> players = await this._context.Players
                                         .Where(player => player.TournamentId == id)
                                         .Include(player => player.Team)
                                         .Include(player => player.Group)
                                         .ToListAsync();
        return this.View(players);
    }

    private async Task LoadTournamentAsync()
    {
        this.ViewBag.Tournament = await this._context.Tournaments.FindAsync(_tournamentId, _userId)
                               ?? throw new NullReferenceException("Tournament not found");
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
    public async Task<IActionResult> Create()
    {
        await this.LoadTournamentAsync();
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
        }")]
        Player player)
    {
        await this.LoadTournamentAsync();
        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();

        if (!this.ModelState.IsValid)
        {
            return this.View(player);
        }

        this._context.Players.Add(player);
        await this._context.SaveChangesAsync();
        this.TempData["Success"] = $"Player {player.PlayerFullName} created successfully!";
        return this.RedirectToAction(nameof(this.Index), new { id = player.TournamentId });
    }

    /// <summary>
    ///     GET: Players/Edit.
    /// </summary>
    /// <param name="id">The id of the player to edit.</param>
    /// <returns>Edit player view.</returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();
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
        await this.LoadTournamentAsync();
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
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();
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
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await this.LoadTournamentAsync();
        await this.LoadTeamsAsync();
        await this.LoadGroupsAsync();

        Player? player = await this._context.Players.FindAsync(id, _tournamentId, _userId);
        this._context.Players.Remove(player ?? throw new InvalidOperationException("Player is null"));
        await this._context.SaveChangesAsync();

        this.TempData["Success"] = $"Player {player.PlayerFullName} deleted successfully!";
        return this.RedirectToAction(nameof(this.Index), new { id = player.TournamentId });
    }

    /// <summary>
    ///     GET: Players/Details.
    /// </summary>
    /// <param name="id">The id of the player to show.</param>
    /// <returns>Details view.</returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        await this.LoadTournamentAsync();
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
