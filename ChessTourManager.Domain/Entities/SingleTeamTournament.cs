using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTeamTournament : TournamentBase, ITeamTournament, IDrawable<Player>
{
    private readonly HashSet<Team> _teams;

    public SingleTeamTournament(Id<Guid>                             id,
                                Name                                 name,
                                DrawSystem                           drawSystem,
                                IReadOnlyCollection<DrawCoefficient> coefficients,
                                TourNumber                           maxTour,
                                DateOnly                             createdAt,
                                bool                                 allowInGroupGames)
        : base(id, name, createdAt)
    {
        this.Kind  = TournamentKind.SingleTeam;
        this.Teams = new HashSet<Team>(new INameable.ByNameEqualityComparer<Team>());
        ((IDrawable<Player>)this).SetTours(maxTour, 1);
        ((IDrawable<Player>)this).SetDrawingProperties(drawSystem, coefficients);
        this.AllowInGroupGames = allowInGroupGames;
    }

    public IReadOnlySet<Team> Teams
    {
        get => this._teams;
        init => this._teams = new HashSet<Team>(value, new INameable.ByNameEqualityComparer<Team>());
    }

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
                                    this.AllowInGroupGames)
               {
                   Groups    = this.Groups,
                   GamePairs = this.GamePairs
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
                                  this.AllowInGroupGames)
               {
                   Teams     = this.Teams,
                   GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>(),
                   Groups    = this.Groups
               };
    }

    public override SingleTeamTournament ConvertToSingleTeamTournament()
    {
        return this;
    }

    public DrawSystem System { get; set; }

    public IReadOnlyCollection<DrawCoefficient> Coefficients { get; set; }

    public TourNumber MaxTour { get; set; }

    public TourNumber CurrentTour { get; set; }

    public bool AllowInGroupGames { get; set; }

    public IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>> GamePairs { get; set; }

    public DrawResult DrawSwiss()
    {
        return DrawResult.Fail("Not implemented");
    }

    public DrawResult DrawRoundRobin()
    {
        return DrawResult.Fail("Not implemented");
    }
}
