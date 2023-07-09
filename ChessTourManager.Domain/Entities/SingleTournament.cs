using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTournament : TournamentBase
{
    internal SingleTournament(Id                               id,
                              Name                             name,
                              DrawSystem                       drawSystem,
                              IReadOnlyCollection<Coefficient> coefficients,
                              TourNumber                       maxTour,
                              DateOnly                         createdAt,
                              TourNumber                       currentTour,
                              List<Group>                      groups)
        : base(id, name, drawSystem, coefficients, maxTour, createdAt, currentTour, groups)
    {
        this.Kind = Kind.Single;
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
