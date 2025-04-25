using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Group : IEquatable<Group>, INameable
{
	private readonly HashSet<Player> _players;

	internal Group(Id<Guid> id, Name name, HashSet<Player>? players = null)
	{
		Id = id;
		Name = name;
		_players = players ?? new HashSet<Player>(new INameable.ByNameEqualityComparer<Player>());
	}

	public Id<Guid> Id { get; }

	internal IReadOnlySet<Player> Players => _players;

	public bool Equals(Group? other)
		=> other is not null
		   && Id == other.Id
		   && Name == other.Name
		   && Players.SequenceEqual(other.Players);

	public Name Name { get; set; }

	public override bool Equals(object? obj)
		=> obj is Group group && Equals(group);

	public override int GetHashCode()
		=> Id.GetHashCode();

	internal AddPlayerResult TryAddPlayer(Id<Guid> playerId, Name playerName, Gender gender, BirthYear birthYear)
	{
		var isUnique = _players.Add(new Player(playerId, playerName, gender, birthYear));

		return isUnique
			? AddPlayerResult.Success
			: AddPlayerResult.PlayerAlreadyExists;
	}

	internal RemovePlayerResult TryRemovePlayer(Id<Guid> playerId)
	{
		var player = _players.SingleOrDefault(player => player.Id == playerId);
		if (player is null)
		{
			return RemovePlayerResult.PlayerDoesNotExist;
		}

		if (player.InGames())
		{
			return RemovePlayerResult.PlayerInGames;
		}

		var isRemoved = _players.Remove(player);

		return isRemoved
			? RemovePlayerResult.Success
			: RemovePlayerResult.Fail;
	}
}