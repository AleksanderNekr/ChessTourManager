using System;
using System.Collections.Generic;

namespace ChessTourManagerWpf.Models.Entities;

public class User
{
    public int UserId { get; set; }

    public string UserLastname { get; set; } = null!;

    public string UserFirstname { get; set; } = null!;

    public string UserPatronymic { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PassHash { get; set; } = null!;

    public int TournamentsLim { get; set; }

    public DateOnly RegisterDate { get; set; }

    public TimeOnly RegisterTime { get; set; }

    public virtual ICollection<Tournament> Tournaments { get; } = new List<Tournament>();
}
