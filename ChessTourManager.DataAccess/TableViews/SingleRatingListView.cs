namespace ChessTourManager.DataAccess.TableViews;

public class SingleRatingListView
{
    public int? PlayerId { get; set; }

    public int? TournamentId { get; set; }

    public int? OrganizerId { get; set; }

    public long? PlayerRank { get; set; }

    public string? PlayerLastName { get; set; }

    public string? PlayerFirstName { get; set; }

    public string? PlayerAttribute { get; set; }

    public string? GroupIdent { get; set; }

    public int? WinsCount { get; set; }

    public int? DrawsCount { get; set; }

    public int? LossesCount { get; set; }

    public int? PointsCount { get; set; }

    public decimal? RatioSum1 { get; set; }

    public decimal? RatioSum2 { get; set; }
}
