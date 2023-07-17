using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class SingleTournament : TournamentBase, IDrawable<Player>
{
    public SingleTournament(Id<Guid>                             id,
                            Name                                 name,
                            DrawSystem                           drawSystem,
                            IReadOnlyCollection<DrawCoefficient> coefficients,
                            TourNumber                           maxTour,
                            DateOnly                             createdAt,
                            bool                                 allowInGroupGames)
        : base(id, name, createdAt)
    {
        this.Kind = TournamentKind.Single;
        ((IDrawable<Player>)this).SetTours(maxTour, 1);
        ((IDrawable<Player>)this).SetDrawingProperties(drawSystem, coefficients);
        this.AllowInGroupGames = allowInGroupGames;
    }

    public override SingleTournament ConvertToSingleTournament()
    {
        return this;
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
                   Groups    = this.Groups,
                   GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>(),
                   Teams     = new HashSet<Team>()
               };
    }

    public override SingleTeamTournament ConvertToSingleTeamTournament()
    {
        return new SingleTeamTournament(this.Id,
                                        this.Name,
                                        this.System,
                                        this.Coefficients,
                                        this.MaxTour,
                                        this.CreatedAt,
                                        this.AllowInGroupGames)
               {
                   Groups    = this.Groups,
                   GamePairs = this.GamePairs,
                   Teams     = new HashSet<Team>()
               };
    }

    public DrawSystem System { get; set; }

    public IReadOnlyCollection<DrawCoefficient> Coefficients { get; set; }

    public TourNumber MaxTour { get; set; }

    public TourNumber CurrentTour { get; set; }

    public bool AllowInGroupGames { get; set; }

    public IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>> GamePairs { get; set; }

    public DrawResult DrawSwiss()
    {
        return DrawResult.Fail("Swiss system is not implemented yet.");
    }

    public DrawResult DrawRoundRobin()
    {
        return DrawResult.Fail("Round-robin system is not implemented yet.");
    }
}
