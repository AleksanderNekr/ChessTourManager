using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Interfaces;

public interface IDrawable<TPlayer> where TPlayer : IPlayer<TPlayer>
{
    public DrawSystem System { get; private protected set; }

    public IReadOnlyCollection<DrawCoefficient> Coefficients { get; private protected set; }

    public void UpdateCoefficients(IReadOnlyCollection<DrawCoefficient> coefficients)
    {
        List<DrawCoefficient> wrongCoefficients = coefficients.Except(GetPossibleCoefficients(this.System)).ToList();
        if (wrongCoefficients.Any())
        {
            throw new DomainException($"Wrong coefficients for {this.System} draw system: {
                string.Join(", ", wrongCoefficients)}");
        }

        this.Coefficients = coefficients;
    }

    public TourNumber MaxTour { get; private protected set; }

    public TourNumber CurrentTour { get; private protected set; }

    public void SetTours(TourNumber maxTour, TourNumber currentTour)
    {
        if (maxTour < currentTour)
        {
            throw new DomainException("Max tour must be greater or equal to current tour");
        }

        this.MaxTour = maxTour;
        this.CurrentTour = currentTour;
    }

    public bool AllowInGroupGames { get; }

    public IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<TPlayer>>> GamePairs { get; }

    public static IEnumerable<DrawCoefficient> GetPossibleCoefficients(DrawSystem drawSystem)
    {
        return drawSystem switch
               {
                   DrawSystem.RoundRobin => new List<DrawCoefficient>
                                            {
                                                DrawCoefficient.Berger,
                                                DrawCoefficient.SimpleBerger,
                                            },
                   DrawSystem.Swiss => new List<DrawCoefficient>
                                       {
                                           DrawCoefficient.Buchholz,
                                           DrawCoefficient.TotalBuchholz,
                                       },
                   _ => throw new DomainOutOfRangeException(nameof(drawSystem), drawSystem),
               };
    }

    public void SetDrawingProperties(DrawSystem drawSystem, IReadOnlyCollection<DrawCoefficient> coefficients)
    {
        this.System = drawSystem;
        this.UpdateCoefficients(coefficients);
    }

    public DrawResult DrawNewTour()
    {
        return this.System switch
               {
                   DrawSystem.RoundRobin => this.DrawRoundRobin(),
                   DrawSystem.Swiss      => this.DrawSwiss(),
                   _ => throw new DomainOutOfRangeException(nameof(this.System),
                                                            this.System,
                                                            "Cannot draw – unknown system"),
               };
    }

    private protected DrawResult DrawSwiss();

    private protected DrawResult DrawRoundRobin();
}

public enum DrawCoefficient
{
    Berger,
    SimpleBerger,
    Buchholz,
    TotalBuchholz,
}

public enum DrawSystem
{
    RoundRobin,
    Swiss,
}