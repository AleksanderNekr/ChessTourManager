using System.Security.Claims;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.Domain.Algorithms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WEB.Controllers;

/// <inheritdoc />
/// <summary>
/// Controller for games.
/// </summary>
public class GamesController : Controller
{
    private static   int                _tournamentId;
    private static   int                _userId;
    private readonly ChessTourContext   _context;
    private static   IDrawingAlgorithm? _drawingAlgorithm;
    private static   List<int>?         _tourNumbers;
    private static   int                _currentTour;
    private static   int                _selectedTour;
    private static   Tournament?        _tournament;
    private static   ICoefficient?      _ratioCounter1;
    private static   ICoefficient?      _ratioCounter2;

    /// <summary>
    /// Constructor for GamesController.
    /// </summary>
    /// <param name="context">ChessTourContext.</param>
    public GamesController(ChessTourContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// GET: Games/Index.
    /// </summary>
    /// <param name="id">Tournament id.</param>
    /// <param name="selectedTour">Selected tour number.</param>
    /// <returns>Games index view.</returns>
    [Authorize]
    public async Task<IActionResult> Index(int id, int? selectedTour, int? organizerId)
    {
        if (id != 0)
        {
            _tournamentId     = id;
            _drawingAlgorithm = IDrawingAlgorithm.Initialize(this._context, _tournamentId);
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

        _tourNumbers = this._context.Games
                           .Where(g => g.TournamentId == _tournamentId
                                    && g.OrganizerId  == _userId)
                           .Select(g => g.TourNumber)
                           .Distinct()
                           .ToList();

        _currentTour = _tourNumbers.Count == 0
                           ? 0
                           : _tourNumbers.Max();
        _selectedTour = selectedTour ?? _currentTour;

        IQueryable<Game> games = this._context.Games
                                     .Include(g => g.PlayerBlack)
                                     .Include(g => g.PlayerWhite)
                                     .Where(g => g.TournamentId == _tournamentId
                                              && g.OrganizerId  == _userId
                                              && g.TourNumber   == _selectedTour);
        this.ViewBag.SelectedTour = _selectedTour;

        ApplyRatiosCounters();

        this.LoadTourNumbers();
        return this.View(await games.ToListAsync());
    }

    private static void ApplyRatiosCounters()
    {
        // If Swiss tournament.
        _ratioCounter1 = ICoefficient.Initialize(CoefficientType.Buchholz);
        _ratioCounter2 = ICoefficient.Initialize(CoefficientType.TotalBuchholz);

        // If round-robin tournament.
        if (_tournament.SystemId == 1)
        {
            _ratioCounter1 = ICoefficient.Initialize(CoefficientType.Berger);
            _ratioCounter2 = ICoefficient.Initialize(CoefficientType.SimpleBerger);
        }
    }

    /// <summary>
    /// POST: Games/Create.
    /// </summary>
    /// <returns>Index view.</returns>
    [Authorize]
    public IActionResult Create()
    {
        foreach (Player player in this._context.Players
                                      .Include(player => player.GamesWhiteOpponents)
                                      .ThenInclude(game => game.PlayerBlack)
                                      .Include(player => player.GamesWhiteOpponents)
                                      .ThenInclude(game => game.PlayerWhite)
                                      .Include(player => player.GamesBlackOpponents)
                                      .ThenInclude(game => game.PlayerBlack)
                                      .Include(player => player.GamesBlackOpponents)
                                      .ThenInclude(game => game.PlayerWhite))
        {
            player.RatioSum1 = (decimal)_ratioCounter1?.CalculateCoefficient(player);
            player.RatioSum2 = (decimal)_ratioCounter2?.CalculateCoefficient(player);
        }

        if (_currentTour == _tournament?.ToursCount)
        {
            if (this.TempData != null)
            {
                this.TempData["Success"] = "Congratulations! The Tournament is over!";
            }

            return this.RedirectToAction(nameof(this.Index),
                                         new { id = _tournamentId, selectedTour = _currentTour });
        }

        List<(int, int)> idPairs =
            new(_drawingAlgorithm?
               .StartNewTour(_currentTour)
               .Where(idPair => idPair.Item1 != -1 && idPair.Item2 != -1)
             ?? throw new InvalidOperationException("Drawing algorithm is null"));
        if (idPairs.Count == 0)
        {
            this.TempData["Warning"] = "Not enough players to start a new tour!";
            return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
        }

        foreach ((int, int) idPair in idPairs)
        {
            InsertResult result = IInsertQueries.CreateInstance(this._context)
                                                .TryAddGamePair(out Game? game,
                                                                idPair.Item1, idPair.Item2,
                                                                _tournamentId,
                                                                _userId,
                                                                _drawingAlgorithm.NewTourNumber);

            if (result == InsertResult.Fail)
            {
                this.TempData["Error"] = "Error while adding game pair! Maybe pair with such data already exists,"
                                       + " or you didn't fill in important data";
                return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
            }
        }

        _currentTour++;
        if (this.TempData is not null)
        {
            this.TempData["Success"] = $"Tour {_currentTour} was successfully drawn!";
        }

        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId });
    }

    /// <summary>
    /// POST: Games/Edit.
    /// </summary>
    /// <param name="whiteId">White player id.</param>
    /// <param name="blackId">Black player id.</param>
    /// <param name="result">Game result.</param>
    /// <returns>Index view.</returns>
    /// <exception cref="NullReferenceException">Player not found.</exception>
    [Authorize]
    public async Task<IActionResult> Edit(
        int    whiteId,
        int    blackId,
        string result)
    {
        Game? game = await this._context.Games.FindAsync(whiteId, blackId, _tournamentId, _userId);
        if (game is null)
        {
            return this.NotFound();
        }

        game.PlayerWhite ??= await this._context.Players.FindAsync(whiteId, _tournamentId, _userId)
                          ?? throw new NullReferenceException("Player not found");

        game.PlayerBlack ??= await this._context.Players.FindAsync(blackId, _tournamentId, _userId)
                          ?? throw new NullReferenceException("Player not found");

        game.TourNumber   = _selectedTour;
        game.TournamentId = _tournamentId;
        game.OrganizerId  = _userId;
        game.Result       = result;

        this._context.Update(game);
        await this._context.SaveChangesAsync();

        return this.RedirectToAction(nameof(this.Index), new { id = _tournamentId, selectedTour = _selectedTour });
    }

    private async Task<IActionResult> LoadTournamentAsync()
    {
        Tournament? tournament = await this._context.Tournaments.FindAsync(_tournamentId, _userId);
        if (tournament is null)
        {
            this.TempData["Error"] = "Tournament not found";
            return this.RedirectToAction("Index", "Tournaments");
        }

        _tournament             = tournament;
        this.ViewBag.Tournament = tournament;
        return this.Ok();
    }

    private void LoadTourNumbers()
    {
        this.ViewBag.TourNumbers = _tourNumbers?.Select(tourNumber => new SelectListItem
                                                                      {
                                                                          Value    = tourNumber.ToString(),
                                                                          Text     = tourNumber + " tour",
                                                                          Selected = tourNumber == _selectedTour
                                                                      })
                                                .OrderBy(item => item.Value)
                                                .ToList()
                                ?? new List<SelectListItem>();
    }
}
