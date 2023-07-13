using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTeamTournament : TournamentBase, ITeamTournament
{
    internal SingleTeamTournament(Id<Guid>                                  id,
                                  Name                                      name,
                                  DrawSystem                                drawSystem,
                                  IReadOnlyCollection<DrawCoefficient>      coefficients,
                                  TourNumber                                maxTour,
                                  DateOnly                                  createdAt,
                                  TourNumber                                currentTour,
                                  ICollection<Group>                        groups,
                                  ICollection<Team>                         teams,
                                  bool                                      allowInGroupGames,
                                  Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
        : base(id, name, drawSystem, coefficients, maxTour, createdAt, currentTour, groups, allowInGroupGames, gamePairs)
    {
        this.Kind  = TournamentKind.SingleTeam;
        this.Teams = teams;
    }

    public ICollection<Team> Teams { get; }

    private protected override DrawResult DrawSwiss()
    {
        throw new NotImplementedException();
    }

    private protected override DrawResult DrawRoundRobin()
    {
        throw new NotImplementedException();
    }
}
