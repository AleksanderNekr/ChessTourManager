using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ChessTourManager.DataAccess.Entities;

public class User : IdentityUser<int>
{
    public override int Id { get; set; }

    [MaxLength(50, ErrorMessage = "The last name must be no more than 50 characters long.")]
    [MinLength(2, ErrorMessage = "The last name must be at least 2 characters long.")]
    [Required]
    [Display(Name = "Last name")]
    // Regular expression for any letter culture invariant
    [RegularExpression(@"^[A-Z][a-z]+$",
                       ErrorMessage = "The last name must contain only letters.")]
    [DataType(DataType.Text)]
    [PersonalData]
    public string? UserLastName { get; set; }

    [MaxLength(50, ErrorMessage = "The first name must be no more than 50 characters long.")]
    [MinLength(2, ErrorMessage = "The first name must be at least 2 characters long.")]
    [Required]
    [Display(Name = "First name")]
    [RegularExpression(@"^[A-Z][a-z]+$",
                       ErrorMessage = "The first name must contain only letters.")]
    [DataType(DataType.Text)]
    [PersonalData]
    public string? UserFirstName { get; set; }

    [MaxLength(50, ErrorMessage = "The patronymic must be no more than 50 characters long.")]
    [MinLength(2, ErrorMessage = "The patronymic must be at least 2 characters long.")]
    [Display(Name = "Patronymic")]
    [RegularExpression(@"^[A-Z][a-z]+$",
                       ErrorMessage = "The patronymic must contain only letters.")]
    [DataType(DataType.Text)]
    [PersonalData]
    public string? UserPatronymic { get; set; }

    [MaxLength(255, ErrorMessage = "The email must be no more than 255 characters long.")]
    [MinLength(2, ErrorMessage = "The email must be at least 2 characters long.")]
    [Required]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [PersonalData]
    [EmailAddress]
    public override string? Email { get; set; }

    [ProtectedPersonalData]
    public override string? PasswordHash { get; set; }

    public int TournamentsLim { get; set; }

    public DateOnly RegisterDate { get; set; }

    public TimeOnly RegisterTime { get; set; }

    public ICollection<Tournament> Tournaments { get; } = new List<Tournament>();
}
