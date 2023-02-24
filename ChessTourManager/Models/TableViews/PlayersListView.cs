namespace ChessTourManagerWpf.Models.TableViews;

public class PlayersListView
{
    public int? PlayerId { get; set; }

    public int? TournamentId { get; set; }

    public int? OrganizerId { get; set; }

    public string? PlayerLastName { get; set; }

    public string? PlayerFirstName { get; set; }

    public char? Gender { get; set; }

    public string? PlayerAttribute { get; set; }

    public string? GroupIdent { get; set; }

    public string? TeamName { get; set; }

    public int? PlayerBirthYear { get; set; }

    public bool? IsActive { get; set; }
}
