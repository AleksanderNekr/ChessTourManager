using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChessTourManager.DataAccess.Entities;

public class Group
{
    public int Id { get; set; }

    public int TournamentId { get; set; }

    public int    OrganizerId { get; set; }

    [MinLength(1, ErrorMessage = "The group identity must be at least 1 character long.")]
    [MaxLength(50, ErrorMessage = "The group identity must be no more than 50 characters long.")]
    public string Identity    { get; set; } = null!;

    [DisplayName("Group Name")]
    [MinLength(2, ErrorMessage = "The group name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "The group name must be no more than 50 characters long.")]
    [Required]
    [RegularExpression(@"^[A-Z][a-z]+$",
                       ErrorMessage = "The group name must start with a capital letter and contain only letters.")]
    public string GroupName { get; set; } = null!;

    public ICollection<Player> Players { get; } = new List<Player>();

    public Tournament? Tournament { get; set; }
}
