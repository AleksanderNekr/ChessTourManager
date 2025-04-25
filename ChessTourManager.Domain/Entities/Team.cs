using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Team : Participant<Team>, IEquatable<Team>, INameable
{
	private readonly LinkedList<Player> _players;

	internal Team(Id<Guid> id, Name name, bool isActive = true, IEnumerable<Player>? players = default)
		: base(id, name, isActive)
	{
		_players = new LinkedList<Player>(players ?? Enumerable.Empty<Player>());
	}

	internal IEnumerable<Player> Players => _players;

	public bool Equals(Team? other)
		=> other is not null
		   && Id == other.Id
		   && Name == other.Name
		   && Players.SequenceEqual(other.Players);

	internal override void SetActive()
	{
		base.SetActive();
		foreach (var player in _players)
		{
			player.SetActive();
		}
	}

	internal override void SetInactive()
	{
		base.SetInactive();
		foreach (var player in _players)
		{
			player.SetInactive();
		}
	}

	public override bool Equals(object? obj)
		=> obj is Team team && Equals(team);

	public override int GetHashCode()
		=> Id.GetHashCode();
}