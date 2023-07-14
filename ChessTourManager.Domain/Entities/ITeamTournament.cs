namespace ChessTourManager.Domain.Entities;

public interface ITeamTournament
{
    public IReadOnlySet<Team> Teams { get; }

    public bool TryAddTeam(Team team);

    public bool TryRemoveTeam(Team team);
}
