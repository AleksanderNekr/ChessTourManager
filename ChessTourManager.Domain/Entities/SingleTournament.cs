using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTournament : TournamentBase
{
    internal SingleTournament(Id<Guid>                                  id,
                              Name                                      name,
                              DrawSystem                                drawSystem,
                              IReadOnlyCollection<DrawCoefficient>      coefficients,
                              TourNumber                                maxTour,
                              DateOnly                                  createdAt,
                              TourNumber                                currentTour,
                              List<Group>                               groups,
                              bool                                      allowInGroupGames,
                              Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
        : base(id, name, drawSystem, coefficients, maxTour, createdAt, currentTour, groups, allowInGroupGames, gamePairs)
    {
        this.Kind = TournamentKind.Single;
    }

    private protected override DrawResult DrawSwiss()
    {
        throw new NotImplementedException();
    }

    private protected override DrawResult DrawRoundRobin()
    {
        throw new NotImplementedException();
    }
}
