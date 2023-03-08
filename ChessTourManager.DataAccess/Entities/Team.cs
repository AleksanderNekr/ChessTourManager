using System.Collections.Generic;
using System.Windows.Input;

namespace ChessTourManager.DataAccess.Entities;

public class Team
{
    public int TeamId { get; set; }

    public int OrganizerId { get; set; }

    public int TournamentId { get; set; }

    public string TeamName { get; set; } = null!;

    public string TeamAttribute { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<Player> Players { get; } = new List<Player>();

    public virtual Tournament Tournament { get; set; } = null!;

}
