namespace ChessTourManager.Domain.Entities.Tests;

public class TournamentBaseTests
{
    [Fact]
    public void TournamentBase_ConstructedWithValues_PropertiesSetCorrectly()
    {
        // Arrange
        Id<Guid> id        = Guid.NewGuid();
        var      name      = new Name("Test Tournament");
        var      createdAt = new DateOnly(2023, 7, 20);

        // Act
        var tournament = new TestTournament(id, name, createdAt);

        // Assert
        Assert.Equal(id,                    tournament.Id);
        Assert.Equal(name,                  tournament.Name);
        Assert.Equal(createdAt,             tournament.CreatedAt);
        Assert.Equal(TournamentKind.Single, tournament.Kind);
    }

    [Fact]
    public void TournamentBase_TryAddGroup_AddsGroupToGroups()
    {
        // Arrange
        Id<Guid> id         = Guid.NewGuid();
        var      name       = new Name("Test Tournament");
        var      createdAt  = new DateOnly(2023, 7, 20);
        var      tournament = new TestTournament(id, name, createdAt);

        // Act
        AddGroupResult result = tournament.TryAddGroup(id, "Group A");

        // Assert
        Assert.Equal(AddGroupResult.Success, result);
        Assert.Contains(new Group(id, "Group A"), tournament.Groups);
    }

    [Fact]
    public void TournamentBase_TryRemoveGroup_RemovesGroupFromGroups()
    {
        // Arrange
        Id<Guid> id         = Guid.NewGuid();
        var      name       = new Name("Test Tournament");
        var      createdAt  = new DateOnly(2023, 7, 20);
        var      tournament = new TestTournament(id, name, createdAt);
        tournament.TryAddGroup(id, "Group A");

        // Act
        RemoveGroupResult result = tournament.TryRemoveGroup(id);

        // Assert
        Assert.Equal(RemoveGroupResult.Success, result);
        Assert.DoesNotContain(new Group(id, "Group A"), tournament.Groups);
    }

    private class TestTournament : TournamentBase
    {
        public TestTournament(Id<Guid> id, Name name, DateOnly createdAt)
            : base(id, name, createdAt)
        {
            Kind = TournamentKind.Single;
        }

        public override SingleTournament ConvertToSingleTournament()
        {
            // Implementation for SingleTournament conversion
            throw new NotImplementedException();
        }

        public override TeamTournament ConvertToTeamTournament()
        {
            // Implementation for TeamTournament conversion
            throw new NotImplementedException();
        }

        public override SingleTeamTournament ConvertToSingleTeamTournament()
        {
            // Implementation for SingleTeamTournament conversion
            throw new NotImplementedException();
        }
    }
}
