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
                            bool                                                           allowInTeamGames  = false)
        : base(id, name, createdAt, allowMixGroupGames, drawSystem, coefficients, maxTour, currentTour, gamePairs)
    {
        this.Kind              = TournamentKind.Team;
        this.Teams             = teams ?? new HashSet<Team>();
        this.AllowInTeamGames = allowInTeamGames;
    }

    public IReadOnlySet<Team> Teams
    {
        get => this._teams;
        internal init => this._teams = new HashSet<Team>(value, new INameable.ByNameEqualityComparer<Team>());
    }

    public bool AllowInTeamGames { get; }

    public bool TryAddTeam(Team team)
    {
        return this._teams.Add(team);
    }

    public bool TryRemoveTeam(Team team)
    {
        return this._teams.Remove(team);
    }

    public override SingleTournament ConvertToSingleTournament()
    {
        return new SingleTournament(this.Id,
                                    this.Name,
                                    this.System,
                                    this.Coefficients,
                                    this.MaxTour,
                                    this.CreatedAt,
                                    this.AllowMixGroupGames,
                                    this.GetPlayersPairings(),
                                    this.CurrentTour)
               {
                   Groups = this.Groups,
               };
    }

    public override TeamTournament ConvertToTeamTournament()
    {
        return this;
    }

    public override SingleTeamTournament ConvertToSingleTeamTournament()
    {
        return new SingleTeamTournament(this.Id,
                                        this.Name,
                                        this.System,
                                        this.Coefficients,
                                        this.MaxTour,
                                        this.CreatedAt,
                                        this.AllowMixGroupGames,
                                        teams: this.Teams,
                                        currentTour: this.CurrentTour,
                                        allowInTeamGames: this.AllowInTeamGames,
                                        gamePairs: this.GetPlayersPairings())
               {
                   Groups = this.Groups
               };
    }

    internal IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>> GetPlayersPairings()
    {

        return this.GamePairs
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
