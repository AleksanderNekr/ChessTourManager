namespace ChessTourManagerWpf.Models.Entities;

public class Game
{
    public int WhiteId { get; set; }

    public int BlackId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public int TourNumber { get; set; }

    public int WhitePoints { get; set; }

    public int BlackPoints { get; set; }

    public bool IsPlayed { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual Player PlayerNavigation { get; set; } = null!;
}
