using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChessTourManager.DataAccess.Entities;

public class User : IdentityUser<int>
{
    public override int Id { get; set; }

    public string? UserLastName { get; set; }

    public string? UserFirstName { get; set; }

    public string? UserPatronymic { get; set; }

    public override string? Email { get; set; }

    public override string? PasswordHash { get; set; }

    public int TournamentsLim { get; set; }

    public DateOnly RegisterDate { get; set; }

    public TimeOnly RegisterTime { get; set; }

    public ICollection<Tournament> Tournaments { get; } = new List<Tournament>();
}
