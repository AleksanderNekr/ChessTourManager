using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ChessTourManager.DataAccess.Entities;

public class Team
{
    private string _teamName = null!;

    [DisplayName("Team")]
    public int Id { get; set; }

    [DisplayName("Organizer")]
    public int OrganizerId { get; set; }

    public int TournamentId { get; set; }

    [DisplayName("Team Name")]
    [MinLength(2, ErrorMessage = "The team name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "The team name must be no more than 50 characters long.")]
    [Required]
    [RegularExpression(@"^[\w|\s]+$",
                       ErrorMessage = "The team name must contain only letters.")]
    public string TeamName
    {
        get { return this._teamName; }
        set { this._teamName = Regex.Replace(value, @"\s+", " "); }
    }

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
