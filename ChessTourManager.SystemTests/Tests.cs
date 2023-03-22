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

namespace ChessTourManager.SystemTests;

public class Tests
{
    private RegisterViewModel         _registerViewModel;
    private LoginViewModel            _loginViewModel;
    private TournamentsListViewModel  _tournamentsListViewModel;
    private CreateTournamentViewModel _createTournamentViewModel;
    private PlayersViewModel          _playersViewModel;
    private AddPlayerViewModel        _addPlayerViewModel;
    private ManageGroupsViewModel     _manageGroupsViewModel;
    private ManageTeamsViewModel      _manageTeamsViewModel;
    private PairsGridViewModel        _pairsGridViewModel;
    private ManageRatingsViewModel    _manageRatingsViewModel;

    [SetUp]
    public void Setup()
    {
        _pairsGridViewModel        = new PairsGridViewModel();
        _manageRatingsViewModel    = new ManageRatingsViewModel();
        _manageGroupsViewModel     = new ManageGroupsViewModel();
        _manageTeamsViewModel      = new ManageTeamsViewModel();
        _playersViewModel          = new PlayersViewModel();
        _addPlayerViewModel        = new AddPlayerViewModel();
        _tournamentsListViewModel  = new TournamentsListViewModel();
        _createTournamentViewModel = new CreateTournamentViewModel();
        _registerViewModel         = new RegisterViewModel();
        _loginViewModel            = new LoginViewModel();
    }

    /// <summary>
    /// Testing scenario:
    /// 1. Register the user.
    /// 2. Enter the account.
    /// 3. Create a tournament.
    /// 4. Open the tournament.
    /// 5. Fill out a list of participants.
    /// 6. Fill in the list of teams.
    /// 7. Fill in the list of groups.
    /// 8. Conduct the draw for the entire tournament.
    /// </summary>
    [Test]
    public void Test()
    {
        string login = "petrov@mail.ru" + DateTime.Now.Ticks;
        RegisterNewUser("Петров", "Петр", login, "123qwe");

        Login(login, "123qwe");

        CreateTournament("Турнир по шахматам");

        OpenTournament("Турнир по шахматам");

        InsertPlayers(6);

        InsertTeams(2);

        InsertGroups(3);

        ConductDraw(5);
    }

    private void ConductDraw(int toursNumber)
    {
        for (var i = 0; i < toursNumber; i++)
        {
            _pairsGridViewModel.CurrentTour = i;
            _pairsGridViewModel.StartNewTour.Execute(null);
            Assert.AreEqual(i + 1, _pairsGridViewModel.CurrentTour);
            Assert.AreEqual(3,     _pairsGridViewModel.Pairs.Count);
        }
    }

    private void InsertGroups(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            _manageGroupsViewModel.GroupName       = $"Группа{i}";
            _manageGroupsViewModel.GroupIdentifier = $"Г{i}";

            var completeAddGroupCommand = new CompleteAddGroupCommand(_manageGroupsViewModel);
            completeAddGroupCommand.Execute(null);
        }
    }

    private void InsertTeams(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            _manageTeamsViewModel.TeamName = $"Команда{i}";

            var completeAddTeamCommand = new CompleteAddTeamCommand(_manageTeamsViewModel);
            completeAddTeamCommand.Execute(null);
        }
    }

    private void InsertPlayers(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            _addPlayerViewModel.PlayerLastName  = $"Иванов{i}";
            _addPlayerViewModel.PlayerFirstName = $"Иван{i}";

            var completeAddPlayerCommand = new CompleteAddPlayerCommand(_addPlayerViewModel);
            completeAddPlayerCommand.Execute(null);
        }
    }

    private void OpenTournament(string name)
    {
        var openTournamentCommand = new OpenTournamentCommand(_tournamentsListViewModel);
        Tournament tournament = _tournamentsListViewModel
                               .TournamentsCollection
                               .Single(t => t.TournamentName == name);
        openTournamentCommand.Execute(tournament);
    }

    private void CreateTournament(string name)
    {
        _createTournamentViewModel.TournamentNameText = name;
        var createTournamentCommand = new CreateTournamentCommand(_createTournamentViewModel);
        createTournamentCommand.Execute(null);
    }

    private void Login(string login, string password)
    {
        _loginViewModel.Login    = login;
        _loginViewModel.Password = password;

        var loginCommand = new LoginCommand(_loginViewModel);
        loginCommand.Execute(null);
    }

    private void RegisterNewUser(string lastName, string firstName, string login, string password)
    {
        _registerViewModel.LastName        = lastName;
        _registerViewModel.FirstName       = firstName;
        _registerViewModel.Email           = login;
        _registerViewModel.PasswordInit    = password;
        _registerViewModel.PasswordConfirm = password;

        var registerCommand = new RegisterCommand(_registerViewModel);
        registerCommand.Execute(null);
    }
}
