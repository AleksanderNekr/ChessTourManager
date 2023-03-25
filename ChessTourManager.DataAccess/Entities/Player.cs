using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTourManager.DataAccess.Entities;

public class Player
{
    public int PlayerId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public string PlayerLastName { get; set; } = null!;

    public string PlayerFirstName { get; set; } = null!;

    public char Gender { get; set; }

    public string PlayerAttribute { get; set; } = null!;

    public int PlayerBirthYear { get; set; } = 2000;

    public bool? IsActive { get; set; }

    public double PointsCount { get; set; }

    public int WinsCount { get; set; }

    public int LossesCount { get; set; }

    public int DrawsCount { get; set; }

    public decimal RatioSum1 { get; set; }

    public decimal RatioSum2 { get; set; }

    public int BoardNumber { get; set; }

    public int? TeamId { get; set; }

    public int? GroupId { get; set; }

    public virtual ICollection<Game> BlackGamePlayers { get; } = new List<Game>();

    public virtual ICollection<Game> WhiteGamePlayers { get; } = new List<Game>();

    public virtual Group? Group { get; set; }

    public virtual Team? Team { get; set; }

    public virtual Tournament Tournament { get; set; } = null!;

    [NotMapped]
    public string PlayerFullName
    {
        get { return PlayerLastName + " " + PlayerFirstName; }
    }

    public override string ToString()
    {
        return PlayerFullName;
    }
}
