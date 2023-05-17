using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTourManager.DataAccess.Entities;

public class Tournament
{
    public Tournament()
    {
        this.IsMixedGroupsLocalized = this.IsMixedGroups is true
                                          ? "Yes"
                                          : "No";
    }

    public int Id { get; set; }

    public int OrganizerId { get; set; }

    [DisplayName("Tournament name")]
    public string TournamentName { get; set; }

    [DisplayName("Tours count")]
    public int ToursCount { get; set; } = 7;

    [DisplayName("Place")]
    public string? Place { get; set; }

    [DisplayName("Date start")]
    public DateOnly DateStart { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [DisplayName("Time start")]
    public TimeOnly TimeStart { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

    [DisplayName("Duration")]
    public int Duration { get; set; } = 3;

    [DisplayName("Max team players")]
    public int MaxTeamPlayers { get; set; } = 5;

    [DisplayName("Organization name")]
    public string? OrganizationName { get; set; }

    [DisplayName("Mixed groups")]
    public bool? IsMixedGroups { get; set; }

    [DisplayName("Date create")]
    public DateOnly DateCreate { get; set; }

    [DisplayName("Time create")]
    public TimeOnly TimeCreate { get; set; }

    [DisplayName("Date last change")]
    public DateOnly DateLastChange { get; set; }

    [DisplayName("Time last change")]
    public TimeOnly TimeLastChange { get; set; }

    [DisplayName("System")]
    public int SystemId { get; set; }

    [DisplayName("Kind")]
    public int KindId { get; set; }

    public ICollection<Group> Groups { get; } = new List<Group>();

    public Kind? Kind { get; set; }

    public User? Organizer { get; set; }

    public ICollection<Player?> Players { get; } = new List<Player?>();

    public System? System { get; set; }

    public ICollection<Team> Teams { get; } = new List<Team>();

    public ICollection<Ratio> Ratios { get; } = new List<Ratio>();

    [NotMapped]
    [DisplayName("Mixed groups")]
    public string IsMixedGroupsLocalized { get; }
}
