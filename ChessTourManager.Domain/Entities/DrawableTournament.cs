using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public abstract class DrawableTournament<TPlayer> : TournamentBase
    where TPlayer : Participant<TPlayer>
{
    protected DrawableTournament(Id<Guid>                                                          id,
                                 Name                                                              name,
                                 DateOnly                                                          createdAt,
                                 bool                                                              allowMixGroupGames,
                                 DrawSystem                                                        drawSystem,
                                 IReadOnlyCollection<DrawCoefficient>                              coefficients,
                                 TourNumber                                                        maxTour,
                                 TourNumber?                                                       currentTour = null,
                                 IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<TPlayer>>>? gamePairs   = default)
        : base(id, name, createdAt)
    {
        AllowMixGroupGames = allowMixGroupGames;
        GamePairs          = gamePairs ?? new Dictionary<TourNumber, IReadOnlySet<GamePair<TPlayer>>>();
        Coefficients       = coefficients;
        SetDrawingProperties(drawSystem, coefficients);
        SetTours(maxTour, currentTour ?? 1);
    }

    public DrawSystem System { get; private set; }

    internal IReadOnlyCollection<DrawCoefficient> Coefficients { get; private set; }

    internal TourNumber MaxTour { get; private set; }

    internal TourNumber CurrentTour { get; private set; }

    internal bool AllowMixGroupGames { get; }

    internal IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<TPlayer>>> GamePairs { get; }

    internal static IEnumerable<DrawCoefficient> GetPossibleCoefficients(DrawSystem drawSystem)
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

    internal void UpdateCoefficients(IReadOnlyCollection<DrawCoefficient> coefficients)
    {
        List<DrawCoefficient> wrongCoefficients = coefficients.Except(GetPossibleCoefficients(System)).ToList();
        if (wrongCoefficients.Any())
        {
            throw new DomainException($"Wrong coefficients for {System} draw system: {
                string.Join(", ", wrongCoefficients)}");
        }

        Coefficients = coefficients;
    }

    internal void SetDrawingProperties(DrawSystem drawSystem, IReadOnlyCollection<DrawCoefficient> coefficients)
    {
        System = drawSystem;
        UpdateCoefficients(coefficients);
    }

    internal void SetTours(TourNumber maxTour, TourNumber currentTour)
    {
        if (maxTour < currentTour)
        {
            throw new DomainException("Max tour must be greater or equal to current tour");
        }

        MaxTour     = maxTour;
        CurrentTour = currentTour;
    }

    internal DrawResult DrawNewTour()
    {
        return System switch
               {
                   DrawSystem.RoundRobin => DrawRoundRobin(),
                   DrawSystem.Swiss      => DrawSwiss(),
                   _ => throw new DomainOutOfRangeException(nameof(System),
                                                            System,
                                                            "Cannot draw – unknown system"),
               };
    }

    private protected abstract DrawResult DrawSwiss();

    private protected abstract DrawResult DrawRoundRobin();
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
