using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

namespace ChessTourManager.WPF.IntegrationTests;

public class Tests
{
    private readonly ChessTourContext _context = new();
    private readonly int              _orgId   = 2;
    private readonly int              _tourId  = 66;

    [Test]
    public void Test1()
    {
        // Get tournament.
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournaments(this._orgId, out List<Tournament>? tournaments);
        Tournament tournament = tournaments?.Single(t => t.Id == this._tourId)
                             ?? throw new InvalidOperationException();

        // Create mock view model.
        var viewModel = new PairsGridViewModel();

        // Create mock TournamentsList view model.
        var tournamentsListViewModel = new MainViewModel();

        // Invoke open tournament command.
        var openTournamentCommand = new OpenTournamentCommand(tournamentsListViewModel);
        openTournamentCommand.Execute(tournament);

        int oldTourNumber = viewModel.CurrentTour;

        // Execute start tour command.
        viewModel.StartNewTour.Execute(null);

        // Check result.
        Assert.AreEqual(oldTourNumber + 1, viewModel.CurrentTour);
    }

    [Test]
    public void Test2()
    {
        // Get tournament.
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournaments(this._orgId, out List<Tournament>? tournaments);
        Tournament tournament = tournaments?.Single(t => t.Id == this._tourId)
                             ?? throw new InvalidOperationException();

        // Create mock login view model.
        var loginViewModel = new LoginViewModel
                             {
                                 Login    = tournament.Organizer.Email,
                                 Password = "123qwe"
                             };
        var loginCommand = new LoginCommand(loginViewModel);

        // Create mock pairs view model.
        var viewModel = new PairsGridViewModel();
        // Create mock TournamentsList view model.
        var tournamentsListViewModel = new MainViewModel();

        // Invoke login command.
        loginCommand.Execute(null);

        // Invoke open tournament command.
        var openTournamentCommand = new OpenTournamentCommand(tournamentsListViewModel);
        openTournamentCommand.Execute(tournament);


        // Add new player.
        IInsertQueries.CreateInstance(this._context)
                      .TryAddPlayer(out Player? _,               this._tourId, this._orgId,
                                    "Test" + DateTime.Now.Ticks, "Player" + DateTime.Now.Ticks);

        int oldTourNumber = viewModel.CurrentTour;

        // Execute start tour command.
        viewModel.StartNewTour.Execute(null);

        // Check result.
        Assert.AreEqual(oldTourNumber + 1, viewModel.CurrentTour);
    }
}
