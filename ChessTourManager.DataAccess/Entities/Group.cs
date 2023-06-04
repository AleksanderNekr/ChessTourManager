using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ChessTourManager.DataAccess.Entities;

public class Group
{
    private string _groupName = null!;
    private string _identity  = null!;

    public int Id { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    [MaxLength(4, ErrorMessage = "The Group Identity must be no more than 4 characters long.")]
    [Required(ErrorMessage = "The Group Identity is required.")]
    public string Identity
    {
        get { return this._identity; }
        set { this._identity = Regex.Replace(value.Trim(), @"\s+", " "); }
    }

    [DisplayName("Group Name")]
    [MinLength(2, ErrorMessage = "The Group Name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "The Group Name must be no more than 50 characters long.")]
    [Required(ErrorMessage = "The Group Name is required.")]
    [RegularExpression(@"^([A-Z]|[a-z]|[А-Я]|[а-я]|\s|\d)+$",
                       ErrorMessage = "The Group Name must contain only letters or digits.")]
    public string GroupName
    {
        get { return this._groupName; }
        set { this._groupName = Regex.Replace(value.Trim(), @"\s+", " "); }
    }

    public ICollection<Player> Players { get; } = new List<Player>();

    public Tournament? Tournament { get; set; }
}
