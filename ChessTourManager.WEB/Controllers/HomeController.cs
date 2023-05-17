using System.Diagnostics;
using ChessTourManager.WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChessTourManager.WEB.Controllers;

/// <inheritdoc />
/// <summary>
/// The controller for the home page.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// The constructor of the controller.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public HomeController(ILogger<HomeController> logger)
    {
        this._logger = logger;
    }

    public IActionResult Index()
    {
        return this.View();
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel
                         { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
