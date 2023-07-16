using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public abstract class TournamentBase : INameable
{
    public enum TournamentKind
    {
        Single,
        Team,
        SingleTeam,
    }

    private readonly HashSet<Group> _groups;

    private protected TournamentBase(Id<Guid> id, Name name, DateOnly createdAt)
    {
        this.Id        = id;
        this.Name      = name;
        this.CreatedAt = createdAt;
        this.Groups   = new HashSet<Group>();
    }

    public Id<Guid> Id { get; }

    public Name Name { get; }

    public DateOnly CreatedAt { get; }

    public TournamentKind Kind { get; private protected init; }

    public required IReadOnlySet<Group> Groups
    {
        get => this._groups;
        init => this._groups = new HashSet<Group>(value, new INameable.ByNameEqualityComparer<Group>());
    }

    public abstract SingleTournament ConvertToSingleTournament();

    public abstract TeamTournament ConvertToTeamTournament();

    public abstract SingleTeamTournament ConvertToSingleTeamTournament();

    public bool TryAddGroup(Group group)
    {
        return this._groups.Add(group);
    }

    public bool TryRemoveGroup(Group group)
    {
        return this._groups.Remove(group);
    }
}
