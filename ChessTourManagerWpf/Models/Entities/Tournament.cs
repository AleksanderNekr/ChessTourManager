using System;
using System.Collections.Generic;

namespace ChessTourManagerWpf.Models.Entities;

public class Tournament
{
    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public string TournamentName { get; set; } = null!;

    public int ToursCount { get; set; }

    public string Place { get; set; } = null!;

    public DateOnly DateStart { get; set; }

    public TimeOnly TimeStart { get; set; }

    public int Duration { get; set; }

    public int MaxTeamPlayers { get; set; }

    public string OrganizationName { get; set; } = null!;

    public bool? IsMixedGroups { get; set; }

    public DateOnly DateCreate { get; set; }

    public TimeOnly TimeCreate { get; set; }

    public DateOnly DateLastChange { get; set; }

    public TimeOnly TimeLastChange { get; set; }

    public int SystemId { get; set; }

    public int KindId { get; set; }

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual Kind Kind { get; set; } = null!;

    public virtual User Organizer { get; set; } = null!;

    public virtual ICollection<Player> Players { get; } = new List<Player>();

    public virtual System System { get; set; } = null!;

    public virtual ICollection<Team> Teams { get; } = new List<Team>();

    public virtual ICollection<Ratio> Ratios { get; } = new List<Ratio>();
}
