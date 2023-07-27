using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class TeamTournament : DrawableTournament<Team>, ITeamTournament
{
    private readonly HashSet<Team> _teams;

    internal TeamTournament(Id<Guid>                                                       id,
                            Name                                                           name,
                            DrawSystem                                                     drawSystem,
                            IReadOnlyCollection<DrawCoefficient>                           coefficients,
                            TourNumber                                                     maxTour,
                            DateOnly                                                       createdAt,
                            bool                                                           allowMixGroupGames = false,
                            TourNumber?                                                    currentTour        = null,
                            IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Team>>>? gamePairs          = default,
                            IReadOnlySet<Team>?                                            teams              = default,
                            bool                                                           allowInTeamGames   = false)
        : base(id, name, createdAt, allowMixGroupGames, drawSystem, coefficients, maxTour, currentTour, gamePairs)
    {
        Kind             = TournamentKind.Team;
        Teams            = teams ?? new HashSet<Team>();
        AllowInTeamGames = allowInTeamGames;
    }

    public IReadOnlySet<Team> Teams
    {
        get => _teams;
        internal init => _teams = new HashSet<Team>(value, new INameable.ByNameEqualityComparer<Team>());
    }

    public bool AllowInTeamGames { get; }

    public bool TryAddTeam(Team team)
    {
        return _teams.Add(team);
    }

    public bool TryRemoveTeam(Team team)
    {
        return _teams.Remove(team);
    }

    public override SingleTournament ConvertToSingleTournament()
    {
        return new SingleTournament(Id,
                                    Name,
                                    System,
                                    Coefficients,
                                    MaxTour,
                                    CreatedAt,
                                    AllowMixGroupGames,
                                    GetPlayersPairings(),
                                    CurrentTour)
               {
                   Groups = Groups,
               };
    }

    public override TeamTournament ConvertToTeamTournament()
    {
        return this;
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
                                        teams: Teams,
                                        currentTour: CurrentTour,
                                        allowInTeamGames: AllowInTeamGames,
                                        gamePairs: GetPlayersPairings())
               {
                   Groups = Groups,
               };
    }

    internal IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>> GetPlayersPairings()
    {

        return GamePairs
           .ToDictionary<KeyValuePair<TourNumber, IReadOnlySet<GamePair<Team>>>,
                TourNumber,
                IReadOnlySet<GamePair<Player>>>(static tourTeamsPairs => tourTeamsPairs.Key,
                                                static tourTeamsPairs => tourTeamsPairs.Value
                                                   .SelectMany(GetPlayersPairs)
                                                   .ToHashSet());

        static IEnumerable<GamePair<Player>> GetPlayersPairs(GamePair<Team> teamPair)
        {
            foreach ((Player First, Player Second) playersPair in teamPair.White.Players.Zip(teamPair.Black.Players))
            {
                yield return new GamePair<Player>(playersPair.First, playersPair.Second, teamPair.Result);
            }
        }
    }

    private protected override DrawResult DrawSwiss()
    {
        return DrawResult.Fail("Not implemented");
    }

    private protected override DrawResult DrawRoundRobin()
    {
        return DrawResult.Fail("Not implemented");
    }
}
