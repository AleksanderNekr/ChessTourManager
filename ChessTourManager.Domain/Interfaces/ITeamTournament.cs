using ChessTourManager.Domain.Entities;

namespace ChessTourManager.Domain.Interfaces;

internal interface ITeamTournament
{
    public IReadOnlySet<Team> Teams { get; }

    public bool AllowInTeamGames { get; }

    public bool TryAddTeam(Team team);

    public bool TryRemoveTeam(Team team);
}
