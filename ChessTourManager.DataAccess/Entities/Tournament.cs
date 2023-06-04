using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ChessTourManager.DataAccess.Entities;

public class Tournament
{
    private string _tournamentName;

    public Tournament()
    {
        this.IsMixedGroupsLocalized = this.IsMixedGroups is true
                                          ? "Yes"
                                          : "No";
    }

    public int Id { get; set; }

    public int OrganizerId { get; set; }

    [DisplayName("Tournament Name")]
    [MinLength(2, ErrorMessage = "The Tournament Name must be at least 2 characters long.")]
    [MaxLength(255, ErrorMessage = "The Tournament Name must be no more than 255 characters long.")]
    [Required(ErrorMessage = "The Tournament Name is required.")]
    [RegularExpression(@"^([A-Za-zА-Яа-я]|\s)+$",
                       ErrorMessage = "The Tournament Name must contain only letters.")]
    public string TournamentName
    {
        get { return this._tournamentName; }
        set { this._tournamentName = Regex.Replace(value, @"\s+", " "); }
    }

    [DisplayName("Tours count")]
    [Range(1, 15, ErrorMessage = "The number of tours must be between 1 and 15.")]
    [Required(ErrorMessage = "The number of tours is required.")]
    public int ToursCount { get; set; } = 7;

    [DisplayName("Place")]
    [MaxLength(255, ErrorMessage = "The Place must be no more than 255 characters long.")]
    public string? Place { get; set; }

    [DisplayName("Date start")]
    [Required(ErrorMessage = "The Date Start is required.")]
    public DateOnly DateStart { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [DisplayName("Time start")]
    [Required(ErrorMessage = "The Time Start is required.")]
    public TimeOnly TimeStart { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

    [DisplayName("Duration")]
    [Range(1, 24)]
    public int Duration { get; set; } = 3;

    [DisplayName("Max team players")]
    [Range(1, 15)]
    public int MaxTeamPlayers { get; set; } = 5;

    [DisplayName("Organization Name")]
    [MaxLength(255, ErrorMessage = "The Organization Name must be no more than 255 characters long.")]
    public string? OrganizationName { get; set; }

    [DisplayName("Mixed groups")]
    public bool? IsMixedGroups { get; set; } = true;

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

    public ICollection<Team>  Teams  { get; } = new List<Team>();
    public ICollection<Ratio> Ratios { get; } = new List<Ratio>();

    [NotMapped]
    [DisplayName("Mixed groups")]
    public string IsMixedGroupsLocalized { get; }
}
