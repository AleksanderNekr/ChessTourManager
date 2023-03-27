using System;
using System.Collections.Generic;

namespace ChessTourManager.DataAccess.Entities;

public class User
{
    public int UserId { get; set; }

    public string? UserLastName { get; set; } = null!;

    public string? UserFirstName { get; set; } = null!;

    public string? UserPatronymic { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string PassHash { get; set; } = null!;

    public int TournamentsLim { get; set; }

    public DateOnly RegisterDate { get; set; }

    public TimeOnly RegisterTime { get; set; }

    public virtual ICollection<Tournament> Tournaments { get; } = new List<Tournament>();
}
