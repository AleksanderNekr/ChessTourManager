using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public abstract class TournamentBase : INameable
{
    private readonly HashSet<Group>   _groups;
    private          HashSet<Player>? _playersWithoutGroup;

    private protected TournamentBase(Id<Guid> id, Name name, DateOnly createdAt)
    {
        this.Id        = id;
        this.Name      = name;
        this.CreatedAt = createdAt;
        this._groups   = new HashSet<Group>(new INameable.ByNameEqualityComparer<Group>());
    }

    public Id<Guid> Id { get; }

    public Name Name { get; }

    internal DateOnly CreatedAt { get; }

    internal TournamentKind Kind { get; init; }

    internal IReadOnlySet<Group> Groups
    {
        get => this._groups;
        init => this._groups = new HashSet<Group>(value, new INameable.ByNameEqualityComparer<Group>());
    }

    public IReadOnlySet<Player> PlayersWithoutGroup
    {
        get => this._playersWithoutGroup ??= new HashSet<Player>(new INameable.ByNameEqualityComparer<Player>());
    }

    public abstract SingleTournament ConvertToSingleTournament();

    public abstract TeamTournament ConvertToTeamTournament();

    public abstract SingleTeamTournament ConvertToSingleTeamTournament();

    internal AddGroupResult TryAddGroup(Id<Guid> groupId, Name groupName)
    {
        bool isUnique = this._groups.Add(new Group(groupId, groupName));

        return isUnique
                   ? AddGroupResult.Success
                   : AddGroupResult.GroupAlreadyExists;
    }

    internal RemoveGroupResult TryRemoveGroup(Id<Guid> groupId)
    {
        Group? group = this._groups.SingleOrDefault(g => g.Id == groupId);
        if (group is null)
        {
            return RemoveGroupResult.GroupDoesNotExist;
        }

        this._playersWithoutGroup ??= new HashSet<Player>(new INameable.ByNameEqualityComparer<Player>());
        foreach (Player player in group.Players)
        {
            this._playersWithoutGroup.Add(player);
        }

        bool removed = this._groups.Remove(group);

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
