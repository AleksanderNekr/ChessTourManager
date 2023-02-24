namespace ChessTourManager.DataAccess.TableViews;

public class TeamsListView
{
    public int? TeamId { get; set; }

    public int? TournamentId { get; set; }

    public int? OrganizerId { get; set; }

    public string? TeamName { get; set; }

    public string? TeamAttribute { get; set; }

    public long? PlayersCount { get; set; }

    public bool? IsActive { get; set; }
}
