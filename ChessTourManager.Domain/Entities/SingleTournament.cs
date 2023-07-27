using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTournament : DrawableTournament<Player>
{
    internal SingleTournament(Id<Guid>                                                         id,
                              Name                                                             name,
                              DrawSystem                                                       drawSystem,
                              IReadOnlyCollection<DrawCoefficient>                             coefficients,
                              TourNumber                                                       maxTour,
                              DateOnly                                                         createdAt,
                              bool                                                             allowMixGroupGames = false,
                              IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>>? gamePairs = default,
                              TourNumber?                                                      currentTour = null)
        : base(id, name, createdAt, allowMixGroupGames, drawSystem, coefficients, maxTour, currentTour, gamePairs)
    {
        Kind = TournamentKind.Single;
    }

    public override SingleTournament ConvertToSingleTournament()
    {
        return this;
    }

    public override TeamTournament ConvertToTeamTournament()
    {
        return new TeamTournament(Id,
                                  Name,
                                  System,
                                  Coefficients,
                                  MaxTour,
                                  CreatedAt,
                                  AllowMixGroupGames)
               {
                   Groups = Groups,
                   Teams  = new HashSet<Team>(),
               };
    }

    public override SingleTeamTournament ConvertToSingleTeamTournament()
    {
        return new SingleTeamTournament(Id,
                                        Name,
                                        System,
                                        Coefficients,
                                        MaxTour,
                                        CreatedAt,
                                        AllowMixGroupGames,
                                        currentTour: CurrentTour,
                                        gamePairs: GamePairs)
               {
                   Groups = Groups,
               };
    }

    public AddPlayerResult TryAddPlayer(Id<Guid>  groupId, Id<Guid> playerId, Name playerName, Gender gender,
                                        BirthYear birthYear)
    {
        Group? group = Groups.SingleOrDefault(g => g.Id == groupId);

        return group?.TryAddPlayer(playerId, playerName, gender, birthYear) ?? AddPlayerResult.GroupDoesNotExist;
    }

    public RemovePlayerResult TryRemovePlayer(Id<Guid> groupId, Id<Guid> playerId)
    {
        Group? group = Groups.SingleOrDefault(g => g.Id == groupId);

        return group?.TryRemovePlayer(playerId) ?? RemovePlayerResult.GroupDoesNotExist;
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
