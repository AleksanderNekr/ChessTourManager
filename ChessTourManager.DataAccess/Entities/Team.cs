using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTourManager.DataAccess.Entities;

public class Team
{
    [DisplayName("Team")]
    public int Id { get; set; }

    [DisplayName("Organizer")]
    public int OrganizerId { get; set; }

    public int TournamentId { get; set; }

    [DisplayName("Team Name")]
    public string TeamName { get; set; } = null!;

    [DisplayName("Team Attribute")]
    public string? TeamAttribute { get; set; }

    [DisplayName("Is Active")]
    public bool IsActive { get; set; } = true;

    public ICollection<Player> Players { get; } = new List<Player>();

    public Tournament? Tournament { get; set; }

    [NotMapped]
    public string IsActiveLocalized =>
        this.IsActive
            ? "Yes"
            : "No";
}
