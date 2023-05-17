using System.Collections.Generic;
using System.ComponentModel;

namespace ChessTourManager.DataAccess.Entities;

public class Group
{
    public int Id { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public string Identity { get; set; } = null!;

    [DisplayName("Group Name")]
    public string GroupName { get; set; } = null!;

    public ICollection<Player> Players { get; } = new List<Player>();

    public Tournament? Tournament { get; set; }
}
