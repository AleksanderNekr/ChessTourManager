namespace ChessTourManagerWpf.Models.Entities;

public class TeamRatingListView
{
    public int? TeamId { get; set; }

    public int? TournamentId { get; set; }

    public int? OrganizerId { get; set; }

    public long? TeamRank { get; set; }

    public string? TeamName { get; set; }

    public string? TeamAttribute { get; set; }

    public long? WinsCount { get; set; }

    public long? DrawsCount { get; set; }

    public long? LossesCount { get; set; }

    public long? PointsCount { get; set; }

    public decimal? RatioSum1 { get; set; }

    public decimal? RatioSum2 { get; set; }
}
