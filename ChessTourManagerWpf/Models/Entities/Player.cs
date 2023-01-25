using System.Collections.Generic;

namespace ChessTourManagerWpf.Models.Entities;

public class Player
{
    public int PlayerId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public string PlayerLastName { get; set; } = null!;

    public string PlayerFirstName { get; set; } = null!;

    public char Gender { get; set; }

    public string PlayerAttribute { get; set; } = null!;

    public int PlayerBirthYear { get; set; }

    public bool? IsActive { get; set; }

    public int PointsCount { get; set; }

    public int WinsCount { get; set; }

    public int LossesCount { get; set; }

    public int DrawsCount { get; set; }

    public decimal RatioSum1 { get; set; }

    public decimal RatioSum2 { get; set; }

    public int BoardNumber { get; set; }

    public int? TeamId { get; set; }

    public int? GroupId { get; set; }

    public virtual ICollection<Game> GamePlayerNavigations { get; } = new List<Game>();

    public virtual ICollection<Game> GamePlayers { get; } = new List<Game>();

    public virtual Group? Group { get; set; }

    public virtual Team? Team { get; set; }

    public virtual Tournament Tournament { get; set; } = null!;
}
