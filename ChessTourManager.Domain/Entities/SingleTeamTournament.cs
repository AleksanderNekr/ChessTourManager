using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTeamTournament : TournamentBase, ITeamTournament
{
    internal SingleTeamTournament(Id                               id,
                                  Name                             name,
                                  DrawSystem                       drawSystem,
                                  IReadOnlyCollection<Coefficient> coefficients,
                                  TourNumber                       maxTour,
                                  DateOnly                         createdAt,
                                  TourNumber                       currentTour,
                                  List<Group>                      groups,
                                  List<Team>                       teams)
        : base(id, name, drawSystem, coefficients, maxTour, createdAt, currentTour, groups)
    {
        this.Kind  = Kind.SingleTeam;
        this.Teams = teams;
    }

    public List<Team> Teams { get; }

    private protected override DrawResult DrawSwiss()
    {
        throw new NotImplementedException();
    }

    private protected override DrawResult DrawRoundRobin()
    {
        throw new NotImplementedException();
    }
}
