namespace ChessTourManagerWpf.Models.Entities;

public class TeamView
{
    public int? TeamId { get; set; }

    public int? TournamentId { get; set; }

    public int? OrganizerId { get; set; }

    public int? PlayerId { get; set; }

    public int? BoardNumber { get; set; }

    public string? PlayerLastName { get; set; }

    public string? PlayerFirstName { get; set; }

    public string? GroupIdent { get; set; }

    public bool? IsActive { get; set; }
}
