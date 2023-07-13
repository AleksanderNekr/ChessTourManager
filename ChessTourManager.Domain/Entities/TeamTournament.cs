﻿using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class TeamTournament : TournamentBase, ITeamTournament
{
    internal TeamTournament(Id<Guid>                                  id,
                            Name                                      name,
                            DrawSystem                                drawSystem,
                            IReadOnlyCollection<DrawCoefficient>      coefficients,
                            TourNumber                                maxTour,
                            DateOnly                                  createdAt,
                            TourNumber                                currentTour,
                            List<Group>                               groups,
                            List<Team>                                teams,
                            bool                                      allowInGroupGames,
                            Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
        : base(id, name, drawSystem, coefficients, maxTour, createdAt, currentTour, groups, allowInGroupGames, gamePairs)
    {
        this.Kind  = TournamentKind.Team;
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
