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
		Kind = TournamentKind.SingleTeam;
		AllowInTeamGames = allowInTeamGames;
		Teams = new HashSet<Team>(teams ?? new HashSet<Team>(), new INameable.ByNameEqualityComparer<Team>());
	}

	public IReadOnlySet<Team> Teams
	{
		get => _teams;
		private init => _teams = new HashSet<Team>(value, new INameable.ByNameEqualityComparer<Team>());
	}

	public bool AllowInTeamGames { get; }

	public bool TryAddTeam(Team team)
		=> _teams.Add(team);

	public bool TryRemoveTeam(Team team)
		=> _teams.Remove(team);

	public override SingleTournament ConvertToSingleTournament()
		=> new(Id,
			Name,
			System,
			Coefficients,
			MaxTour,
			CreatedAt,
			AllowMixGroupGames,
			GamePairs,
			CurrentTour)
		{
			Groups = Groups,
		};

	public override TeamTournament ConvertToTeamTournament()
		=> new(Id,
			Name,
			System,
			Coefficients,
			MaxTour,
			CreatedAt,
			AllowMixGroupGames)
		{
			Teams = Teams,
			Groups = Groups,
		};

	public override SingleTeamTournament ConvertToSingleTeamTournament()
		=> this;

	private protected override DrawResult DrawSwiss()
		=> DrawResult.Fail("Not implemented");

	private protected override DrawResult DrawRoundRobin()
		=> DrawResult.Fail("Not implemented");
}