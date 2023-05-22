using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    [MinLength(2, ErrorMessage = "The team name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "The team name must be no more than 50 characters long.")]
    [Required]
    [RegularExpression(@"^\w+$",
                       ErrorMessage = "The team name must start with a capital letter and contain only letters.")]
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
