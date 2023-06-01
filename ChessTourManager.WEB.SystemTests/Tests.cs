namespace ChessTourManager.WEB.SystemTests;

public class Tests
{
    private          TournamentsController _tournamentsController;
    private          LoginModel            _loginModel;
    private          RegisterModel         _registerModel;
    private          PlayersController     _playersController;
    private          TeamsController       _teamsController;
    private          GroupsController      _groupsController;
    private          GamesController       _gamesController;
    private readonly ChessTourContext      _context = new();

    /// <summary>
    ///     Testing scenario:
    ///     1. Register the user.
    ///     2. Create a tournament.
    ///     3. Open the tournament.
    ///     4. Fill out a list of participants.
    ///     5. Fill in the list of teams.
    ///     6. Fill in the list of groups.
    ///     7. Conduct the draw for the entire tournament.
    /// </summary>
    [Test]
    public async Task Test()
    {
        User user = await this.RegisterUserAsync();

        Tournament tournament = await this.CreateTournamentAsync(user);

        await this.OpenTournamentAsync(tournament);

        await this.InsertPlayersAsync(10, tournament);

        await this.InsertTeamsAsync(2, tournament);

        await this.InsertGroupsAsync(3, tournament);

        await this.ConductDrawAsync(5, tournament);
    }

    private async Task ConductDrawAsync(int maxTourNumber, Tournament tournament)
    {
        this._gamesController = new GamesController(this._context);
        await this._gamesController.Index(tournament.Id, null, tournament.OrganizerId);
        for (var i = 1; i < maxTourNumber + 1; i++)
        {
            IGetQueries.CreateInstance(this._context)
                       .TryGetGames(tournament.OrganizerId, tournament.Id, out List<Game>? games);
            int oldTourNumber = games?.Count > 0
                                    ? games.Max(g => g.TourNumber)
                                    : 0;

            this._gamesController.Create();

            // Check if the games are created.
            IGetQueries.CreateInstance(this._context)
                       .TryGetGames(tournament.OrganizerId, tournament.Id, out games);
            int newTourNumber = games?.Count > 0
                                    ? games.Max(g => g.TourNumber)
                                    : 0;
            Assert.AreEqual(oldTourNumber + 1, newTourNumber);
        }
    }

    private async Task InsertGroupsAsync(int amount, Tournament tournament)
    {
        this._groupsController = new GroupsController(this._context);
        await this._groupsController.Index(tournament.Id, tournament.OrganizerId);
        for (var i = 0; i < amount; i++)
        {
            await this._groupsController.Create(new Group
                                                {
                                                    OrganizerId  = tournament.OrganizerId,
                                                    TournamentId = tournament.Id,
                                                    GroupName    = $"Группа{i}",
                                                    Identity     = $"G{i}"
                                                });
        }

        // Check if the groups are created.
        List<Group> groups = await this._context.Groups
                                       .Where(g => g.TournamentId == tournament.Id)
                                       .ToListAsync();
        Assert.AreEqual(amount, groups.Count);
    }

    private async Task InsertTeamsAsync(int amount, Tournament tournament)
    {
        this._teamsController = new TeamsController(this._context);
        await this._teamsController.Index(tournament.Id, tournament.OrganizerId);
        for (var i = 0; i < amount; i++)
        {
            await this._teamsController.Create(new Team
                                               {
                                                   OrganizerId  = tournament.OrganizerId,
                                                   TournamentId = tournament.Id,
                                                   TeamName     = $"Команда{i}"
                                               });
        }

        // Check if the teams are created.
        List<Team> teams = await this._context.Teams
                                     .Where(t => t.TournamentId == tournament.Id)
                                     .ToListAsync();
        Assert.AreEqual(amount, teams.Count);
    }

    private async Task InsertPlayersAsync(int amount, Tournament tournament)
    {
        for (var i = 0; i < amount; i++)
        {
            await this._playersController.Create(new Player
                                                 {
                                                     TournamentId    = tournament.Id,
                                                     OrganizerId     = tournament.OrganizerId,
                                                     PlayerLastName  = "Иванов" + i,
                                                     PlayerFirstName = "Иван"   + i,
                                                 });
        }

        // Check if the players are created.
        List<Player> players = await this._context.Players
                                         .Where(p => p.TournamentId == tournament.Id)
                                         .ToListAsync();
        Assert.AreEqual(amount, players.Count);
    }

    private async Task OpenTournamentAsync(Tournament tournament)
    {
        this._playersController = new PlayersController(this._context);
        await this._playersController.Index(tournament.Id, tournament.OrganizerId);
    }

    private async Task<Tournament> CreateTournamentAsync(User user)
    {
        var name = Guid.NewGuid().ToString();
        this._tournamentsController = new TournamentsController(this._context);
        await this._tournamentsController.Index(user.Id);
        await this._tournamentsController.Create(new Tournament
                                                 {
                                                     OrganizerId    = user.Id,
                                                     TournamentName = name,
                                                     SystemId       = 1,
                                                     KindId         = 1
                                                 });
        // Check if the tournament is created.
        Tournament tournament = await this._context.Tournaments
                                          .SingleAsync(t =>
                                                           t.OrganizerId    == user.Id
                                                        && t.TournamentName == name);
        Assert.NotNull(tournament);
        Assert.AreNotEqual(0, tournament!.Id);
        return tournament;
    }

    private async Task<User> RegisterUserAsync()
    {
        User user = new()
                    {
                        UserName       = Guid.NewGuid().ToString(),
                        Email          = Guid.NewGuid() + "@mail.ru",
                        EmailConfirmed = true,
                        UserFirstName  = "Иван"   + Guid.NewGuid(),
                        UserLastName   = "Иванов" + Guid.NewGuid()
                    };
        string password = Guid.NewGuid() + "!Aa1";
        user.PasswordHash       = new PasswordHasher<User>().HashPassword(user, password);
        user.NormalizedUserName = user.UserName.ToUpper();
        user.NormalizedEmail    = user.Email.ToUpper();

        this._context.Users.Add(user);
        await this._context.SaveChangesAsync();

        // Check if the user is created.
        User userFromDb = await this._context.Users.SingleAsync(u => u.UserName == user.UserName);
        Assert.NotNull(userFromDb);
        Assert.AreNotEqual(0, userFromDb.Id);

        return user;
    }
}
