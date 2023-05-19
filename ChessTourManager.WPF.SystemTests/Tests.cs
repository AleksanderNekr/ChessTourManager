using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.Authentication.Register;
using ChessTourManager.WPF.Features.ManageTournaments;
using ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGames;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManageRatings;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

namespace ChessTourManager.WPF.SystemTests;

public class Tests
{
    private AddPlayerViewModel        _addPlayerViewModel;
    private CreateTournamentViewModel _createTournamentViewModel;
    private LoginViewModel            _loginViewModel;
    private ManageGroupsViewModel     _manageGroupsViewModel;
    private ManageRatingsViewModel    _manageRatingsViewModel;
    private ManageTeamsViewModel      _manageTeamsViewModel;
    private PairsGridViewModel        _pairsGridViewModel;
    private PlayersViewModel          _playersViewModel;
    private RegisterViewModel         _registerViewModel;
    private MainViewModel  _mainViewModel;

    [SetUp]
    public void Setup()
    {
        this._pairsGridViewModel        = new PairsGridViewModel();
        this._manageRatingsViewModel    = new ManageRatingsViewModel();
        this._manageGroupsViewModel     = new ManageGroupsViewModel();
        this._manageTeamsViewModel      = new ManageTeamsViewModel();
        this._playersViewModel          = new PlayersViewModel();
        this._addPlayerViewModel        = new AddPlayerViewModel();
        this._mainViewModel             = new MainViewModel();
        this._createTournamentViewModel = new CreateTournamentViewModel();
        this._registerViewModel         = new RegisterViewModel();
        this._loginViewModel               = new LoginViewModel();
    }

    /// <summary>
    ///     Testing scenario:
    ///     1. Register the user.
    ///     2. Enter the account.
    ///     3. Create a tournament.
    ///     4. Open the tournament.
    ///     5. Fill out a list of participants.
    ///     6. Fill in the list of teams.
    ///     7. Fill in the list of groups.
    ///     8. Conduct the draw for the entire tournament.
    /// </summary>
    [Test]
    public void Test()
    {
        string? login = "petrov@mail.ru" + DateTime.Now.Ticks;
        this.RegisterNewUser("Петров", "Петр", login, "123qwe");

        this.Login(login, "123qwe");

        this.CreateTournament("Турнир по шахматам");

        this.OpenTournament("Турнир по шахматам");

        this.InsertPlayers(6);

        this.InsertTeams(2);

        this.InsertGroups(3);

        this.ConductDraw(5);
    }

    private void ConductDraw(int toursNumber)
    {
        for (var i = 0; i < toursNumber; i++)
        {
            this._pairsGridViewModel.CurrentTour = i;
            this._pairsGridViewModel.StartNewTour.Execute(null);
            Assert.AreEqual(i + 1, this._pairsGridViewModel.CurrentTour);
            Assert.AreEqual(3,     this._pairsGridViewModel.GamesForSelectedTour.Count);
        }
    }

    private void InsertGroups(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            this._manageGroupsViewModel.GroupName    = $"Группа{i}";
            this._manageGroupsViewModel.GroupIdentifier = $"Г{i}";

            var completeAddGroupCommand = new CompleteAddGroupCommand(this._manageGroupsViewModel);
            completeAddGroupCommand.Execute(null);
        }
    }

    private void InsertTeams(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            this._manageTeamsViewModel.TeamName = $"Команда{i}";

            var completeAddTeamCommand = new CompleteAddTeamCommand(this._manageTeamsViewModel);
            completeAddTeamCommand.Execute(null);
        }
    }

    private void InsertPlayers(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            this._addPlayerViewModel.PlayerLastName = $"Иванов{i}";
            this._addPlayerViewModel.PlayerFirstName   = $"Иван{i}";

            var completeAddPlayerCommand = new CompleteAddPlayerCommand(this._addPlayerViewModel);
            completeAddPlayerCommand.Execute(null);
        }
    }

    private void OpenTournament(string name)
    {
        var openTournamentCommand = new OpenTournamentCommand(this._mainViewModel);
        Tournament tournament = this._mainViewModel
                                    .TournamentsCollection
                                    .Single(t => t.TournamentName == name);
        openTournamentCommand.Execute(tournament);
    }

    private void CreateTournament(string? name)
    {
        this._createTournamentViewModel.TournamentNameText = name;
        var createTournamentCommand = new CreateTournamentCommand(this._createTournamentViewModel);
        createTournamentCommand.Execute(null);
    }

    private void Login(string? login, string password)
    {
        this._loginViewModel.Login = login;
        this._loginViewModel.Password = password;

        var loginCommand = new LoginCommand(this._loginViewModel);
        loginCommand.Execute(null);
    }

    private void RegisterNewUser(string lastName, string firstName, string? login, string password)
    {
        this._registerViewModel.LastName     = lastName;
        this._registerViewModel.FirstName    = firstName;
        this._registerViewModel.Email        = login;
        this._registerViewModel.PasswordInit = password;
        this._registerViewModel.PasswordConfirm = password;

        var registerCommand = new RegisterCommand(this._registerViewModel);
        registerCommand.Execute(null);
    }
}
