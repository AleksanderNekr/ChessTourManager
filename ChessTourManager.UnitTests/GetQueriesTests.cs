using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;

namespace ChessTourManager.UnitTests;

[TestFixture]
public class GetQueriesTests
{
    // For each method of IGetQueries interface, we need to test the following cases:
    // 1.	The object to be searched for exists.
    // 1.1.	The object has correct ID.
    // 1.1.1.	The object has no nested collections.
    // 1.1.2.	The object has nested collections.
    // 1.2.	The object has correct login and password.
    // 1.2.1.	The object has no nested collections.
    // 1.2.2.	The object has nested collections.
    // 2.	The object to be searched for does not exist.
    // 2.1.	The object to be searched for does not exist (incorrect ID).
    // 2.2.	The object to be searched for does not exist (incorrect login).
    // 2.3.	The object to be searched for does not exist (incorrect password).


    // 2.1.	The object to be searched for does not exist (incorrect ID).
    [Test]
    public void TryGetUserById_WhenUserDoesNotExist_ReturnsUserNotFound()
    {
        // Arrange.
        var       context = new ChessTourContext();
        var       queries = IGetQueries.CreateInstance(context);
        const int id      = 1;

        // Act.
        GetResult result = queries.TryGetUserById(id, out User? user);

        // Assert.
        Assert.AreEqual(GetResult.UserNotFound, result);
        Assert.IsNull(user);
    }

    // 2.2.	The object to be searched for does not exist (incorrect login).
    [Test]
    public void TryGetUserByLoginAndPass_WhenLoginIsNotCorrect_ReturnsUserNotFound()
    {
        // Arrange.
        var           context = new ChessTourContext();
        var           queries = IGetQueries.CreateInstance(context);
        const string? login   = "incorrect";

        // Users with this password exist in the database.
        const string password = "123qwe";

        // Act.
        GetResult result = queries.TryGetUserByLoginAndPass(login, password, out User? user);

        // Assert.
        Assert.AreEqual(GetResult.UserNotFound, result);
        Assert.IsNull(user);
    }

    // 2.3.	The object to be searched for does not exist (incorrect password).
    [Test]
    public void TryGetUserByLoginAndPass_WhenPasswordIsNotCorrect_ReturnsUserNotFound()
    {
        // Arrange.
        var          context  = new ChessTourContext();
        var          queries  = IGetQueries.CreateInstance(context);
        const string password = "incorrect";

        // User with this login exists in the database.
        const string? login = "petre@live.com";

        // Act.
        GetResult result = queries.TryGetUserByLoginAndPass(login, password, out User? user);

        // Assert.
        Assert.AreEqual(GetResult.UserNotFound, result);
        Assert.IsNull(user);
    }


    // 1.1.1.	The object has no nested collections.
    [Test]
    public void TryGetUserById_WhenUserExists_ReturnsSuccess()
    {
        // Arrange.
        var       context = new ChessTourContext();
        var       queries = IGetQueries.CreateInstance(context);
        const int id      = 2;

        // Act.
        GetResult result = queries.TryGetUserById(id, out User? user);

        // Assert.
        Assert.AreEqual(GetResult.Success, result);
        Assert.IsNotNull(user);
        Assert.AreEqual(id,               user!.UserId);
        Assert.AreEqual("petre@live.com", user.Email);
    }

    // 1.1.2.	The object (tournament) has nested collections.
    [Test]
    public void TryGetTournamentById_WhenTournamentExists_ReturnsSuccess()
    {
        // Arrange.
        var          context              = new ChessTourContext();
        var          queries              = IGetQueries.CreateInstance(context);
        const int    userId               = 2;
        const int    expectedTournamentId = 2;
        const int    expectedTeamId       = 1;
        const string expectedTeamName     = "Медведь";

        // Act.
        GetResult result = queries.TryGetTournamentsWithTeamsAndPlayers(userId,
                                                                        out IEnumerable<Tournament?>? tournaments);
        Tournament? tournament = tournaments!.Single(t => t.TournamentId == expectedTournamentId);
        Team       team       = tournament.Teams.Single(t => t.TeamId   == expectedTeamId);

        // Assert.
        Assert.AreEqual(GetResult.Success, result);
        Assert.IsNotNull(tournaments);
        Assert.AreEqual(expectedTeamName, team.TeamName);
    }
}
