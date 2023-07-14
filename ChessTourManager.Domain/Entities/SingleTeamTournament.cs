using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTeamTournament : TournamentBase, ITeamTournament
{
    private readonly HashSet<Team> _teams;

    internal SingleTeamTournament(Id<Guid>                                                id,
                                  Name                                                    name,
                                  DrawSystem                                              drawSystem,
                                  IReadOnlyCollection<DrawCoefficient>                    coefficients,
                                  TourNumber                                              maxTour,
                                  DateOnly                                                createdAt,
                                  TourNumber                                              currentTour,
                                  IEnumerable<Group>                                      groups,
                                  IEnumerable<Team>                                       teams,
                                  bool                                                    allowInGroupGames,
                                  IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair>> gamePairs)
        : base(id, name, drawSystem, coefficients, maxTour, createdAt, currentTour, groups, allowInGroupGames, gamePairs)
    {
        this.Kind   = TournamentKind.SingleTeam;
        this._teams = teams.ToHashSet(new INameable.ByNameEqualityComparer<Team>());
    }

    public IReadOnlySet<Team> Teams
    {
        get => this._teams;
    }

    public bool TryAddTeam(Team team)
    {
        return this._teams.Add(team);
    }

    public bool TryRemoveTeam(Team team)
    {
        return this._teams.Remove(team);
    }

    private protected override DrawResult DrawSwiss()
    {
        return DrawResult.Fail("Swiss system is not implemented yet.");
    }

    private protected override DrawResult DrawRoundRobin()
    {
        return DrawResult.Fail("Round-robin system is not implemented yet.");
    }
}
