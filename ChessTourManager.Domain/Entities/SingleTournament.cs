using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTournament : TournamentBase
{
    internal SingleTournament(Id<Guid>                                                id,
                              Name                                                    name,
                              DrawSystem                                              drawSystem,
                              IReadOnlyCollection<DrawCoefficient>                    coefficients,
                              TourNumber                                              maxTour,
                              DateOnly                                                createdAt,
                              TourNumber                                              currentTour,
                              IEnumerable<Group>                                      groups,
                              bool                                                    allowInGroupGames,
                              IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair>> gamePairs)
        : base(id, name, drawSystem, coefficients, maxTour, createdAt, currentTour, groups, allowInGroupGames, gamePairs)
    {
        this.Kind = TournamentKind.Single;
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
