using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class TeamTournament : TournamentBase, ITeamTournament, IDrawable<Team>
{
    private readonly HashSet<Team> _teams;

    public TeamTournament(Id<Guid>                             id,
                          Name                                 name,
                          DrawSystem                           drawSystem,
                          IReadOnlyCollection<DrawCoefficient> coefficients,
                          TourNumber                           maxTour,
                          DateOnly                             createdAt,
                          bool                                 allowInGroupGames)
        : base(id, name, createdAt)
    {
        this.Kind  = TournamentKind.Team;
        this.Teams = new HashSet<Team>();
        ((IDrawable<Team>)this).SetTours(maxTour, 1);
        ((IDrawable<Team>)this).SetDrawingProperties(drawSystem, coefficients);
        this.AllowInGroupGames = allowInGroupGames;
    }

    public required IReadOnlySet<Team> Teams
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
                   GamePairs = this.GetPlayersPairings()
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
                                        this.AllowInGroupGames)
               {
                   Teams     = this.Teams,
                   Groups    = this.Groups,
                   GamePairs = this.GetPlayersPairings()
               };
    }

    public IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>> GetPlayersPairings()
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

    public DrawSystem System { get; set; }

    public IReadOnlyCollection<DrawCoefficient> Coefficients { get; set; }

    public TourNumber MaxTour { get; set; }

    public TourNumber CurrentTour { get; set; }

    public bool AllowInGroupGames { get; set; }

    public required IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Team>>> GamePairs { get; set; }

    public DrawResult DrawSwiss()
    {
        return DrawResult.Fail("Not implemented");
    }

    public DrawResult DrawRoundRobin()
    {
        return DrawResult.Fail("Not implemented");
    }
}
