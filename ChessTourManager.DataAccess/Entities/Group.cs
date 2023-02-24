using System.Collections.Generic;

namespace ChessTourManager.DataAccess.Entities;

public class Group
{
    public int GroupId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public string Identity { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public virtual ICollection<Player> Players { get; } = new List<Player>();

    public virtual Tournament Tournament { get; set; } = null!;
}
