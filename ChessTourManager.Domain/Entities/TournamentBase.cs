using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public abstract class TournamentBase : INameable
{
    private readonly HashSet<Group>   _groups;
    private          HashSet<Player>? _playersWithoutGroup;

    private protected TournamentBase(Id<Guid> id, Name name, DateOnly createdAt)
    {
        Id        = id;
        Name      = name;
        CreatedAt = createdAt;
        _groups   = new HashSet<Group>(new INameable.ByNameEqualityComparer<Group>());
    }

    public Id<Guid> Id { get; }

    internal DateOnly CreatedAt { get; }

    internal TournamentKind Kind { get; init; }

    internal IReadOnlySet<Group> Groups
    {
        get => _groups;
        init => _groups = new HashSet<Group>(value, new INameable.ByNameEqualityComparer<Group>());
    }

    public IReadOnlySet<Player> PlayersWithoutGroup
    {
        get => _playersWithoutGroup ??= new HashSet<Player>(new INameable.ByNameEqualityComparer<Player>());
    }

    public Name Name { get; }

    public abstract SingleTournament ConvertToSingleTournament();

    public abstract TeamTournament ConvertToTeamTournament();

    public abstract SingleTeamTournament ConvertToSingleTeamTournament();

    internal AddGroupResult TryAddGroup(Id<Guid> groupId, Name groupName)
    {
        bool isUnique = _groups.Add(new Group(groupId, groupName));

        return isUnique
                   ? AddGroupResult.Success
                   : AddGroupResult.GroupAlreadyExists;
    }

    internal RemoveGroupResult TryRemoveGroup(Id<Guid> groupId)
    {
        Group? group = _groups.SingleOrDefault(g => g.Id == groupId);
        if (group is null)
        {
            return RemoveGroupResult.GroupDoesNotExist;
        }

        _playersWithoutGroup ??= new HashSet<Player>(new INameable.ByNameEqualityComparer<Player>());
        foreach (Player player in group.Players)
        {
            _playersWithoutGroup.Add(player);
        }

        bool removed = _groups.Remove(group);

        return removed
                   ? RemoveGroupResult.Success
                   : RemoveGroupResult.Fail;
    }
}

internal enum TournamentKind
{
    Single,
    Team,
    SingleTeam,
}

public enum AddPlayerResult
{
    Success,
    GroupDoesNotExist,
    PlayerAlreadyExists,
}

public enum RemovePlayerResult
{
    Success,
    GroupDoesNotExist,
    PlayerDoesNotExist,
    PlayerInGames,
    Fail,
}

internal enum AddGroupResult
{
    Success,
    GroupAlreadyExists,
}

internal enum RemoveGroupResult
{
    Success,
    GroupDoesNotExist,
    Fail,
}
