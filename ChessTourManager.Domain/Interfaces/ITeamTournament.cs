using ChessTourManager.Domain.Entities;

namespace ChessTourManager.Domain.Interfaces;

public interface ITeamTournament
{
    public IReadOnlySet<Team> Teams { get; }

    public bool TryAddTeam(Team team);

    public bool TryRemoveTeam(Team team);
}
