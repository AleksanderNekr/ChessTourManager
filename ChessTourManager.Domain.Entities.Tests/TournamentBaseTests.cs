namespace ChessTourManager.Domain.Entities.Tests;

public class TournamentBaseTests
{
    private class TestTournament : TournamentBase
    {
        public TestTournament(Id<Guid> id, Name name, DateOnly createdAt)
            : base(id, name, createdAt)
        {
            this.Kind = TournamentKind.Single;
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
        Assert.Equal(id,        tournament.Id);
        Assert.Equal(name,      tournament.Name);
        Assert.Equal(createdAt, tournament.CreatedAt);
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
        var      group      = new Group(Guid.NewGuid(), new Name("Group A"));

        // Act
        bool result = tournament.TryAddGroup(group);

        // Assert
        Assert.True(result);
        Assert.Contains(group, tournament.Groups);
    }

    [Fact]
    public void TournamentBase_TryRemoveGroup_RemovesGroupFromGroups()
    {
        // Arrange
        Id<Guid> id         = Guid.NewGuid();
        var      name       = new Name("Test Tournament");
        var      createdAt  = new DateOnly(2023, 7, 20);
        var      tournament = new TestTournament(id, name, createdAt);
        var      group      = new Group(Guid.NewGuid(), new Name("Group A"));
        tournament.TryAddGroup(group);

        // Act
        bool result = tournament.TryRemoveGroup(group);

        // Assert
        Assert.True(result);
        Assert.DoesNotContain(group, tournament.Groups);
    }
}
