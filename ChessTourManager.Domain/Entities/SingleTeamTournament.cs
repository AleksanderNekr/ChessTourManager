using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTeamTournament : DrawableTournament<Player>, ITeamTournament
{
    private readonly HashSet<Team> _teams;

    public SingleTeamTournament(Id<Guid> id,
                                Name name,
                                DrawSystem drawSystem,
                                IReadOnlyCollection<DrawCoefficient> coefficients,
                                TourNumber maxTour,
                                DateOnly createdAt,
                                bool allowMixGroupGames = false,
                                bool allowInTeamGames = false,
                                IReadOnlySet<Team>? teams = default,
                                TourNumber? currentTour = null,
                                IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>>? gamePairs = default)
        : base(id, name, createdAt, allowMixGroupGames, drawSystem, coefficients, maxTour, currentTour, gamePairs)
    {
        this.Kind = TournamentKind.SingleTeam;
        this.AllowInTeamGames = allowInTeamGames;
        this.Teams = new HashSet<Team>(teams ?? new HashSet<Team>(), new INameable.ByNameEqualityComparer<Team>());
    }

    public IReadOnlySet<Team> Teams
    {
        get => this._teams;
        private init => this._teams = new HashSet<Team>(value, new INameable.ByNameEqualityComparer<Team>());
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
                                    this.GamePairs,
                                    this.CurrentTour)
               {
                   Groups = this.Groups,
               };
    }

    public override TeamTournament ConvertToTeamTournament()
    {
        return new TeamTournament(this.Id,
                                  this.Name,
                                  this.System,
                                  this.Coefficients,
                                  this.MaxTour,
                                  this.CreatedAt,
                                  this.AllowMixGroupGames)
               {
                   Teams  = this.Teams,
                   Groups = this.Groups
               };
    }

    public override SingleTeamTournament ConvertToSingleTeamTournament()
    {
        return this;
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
