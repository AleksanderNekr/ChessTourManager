using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ChessTourManager.DataAccess.Entities;

public class User : IdentityUser<int>
{
    public override int Id { get; set; }

    [MaxLength(50, ErrorMessage = "The Last Name must be no more than 50 characters long.")]
    [MinLength(2, ErrorMessage = "The Last Name must be at least 2 characters long.")]
    [Required(ErrorMessage = "The Last Name is required.")]
    [Display(Name = "Last Name")]
    // Regular expression for any letter culture invariant
    [RegularExpression(@"^([A-Za-zА-Яа-я]|\s)+$",
                       ErrorMessage = "The Last Name must contain only letters.")]
    [DataType(DataType.Text)]
    [PersonalData]
    public string? UserLastName { get; set; }

    [MaxLength(50, ErrorMessage = "The First Name must be no more than 50 characters long.")]
    [MinLength(2, ErrorMessage = "The First Name must be at least 2 characters long.")]
    [Required(ErrorMessage = "The First Name is required.")]
    [Display(Name = "First Name")]
    [RegularExpression(@"^([A-Za-zА-Яа-я]|\s)+$",
                       ErrorMessage = "The First Name must contain only letters.")]
    [DataType(DataType.Text)]
    [PersonalData]
    public string? UserFirstName { get; set; }

    [MaxLength(50, ErrorMessage = "The Patronymic must be no more than 50 characters long.")]
    [MinLength(2, ErrorMessage = "The Patronymic must be at least 2 characters long.")]
    [Display(Name = "Patronymic")]
    [RegularExpression(@"^([A-Za-zА-Яа-я]|\s)+$",
                       ErrorMessage = "The Patronymic must contain only letters.")]
    [DataType(DataType.Text)]
    [PersonalData]
    public string? UserPatronymic { get; set; }

    [MaxLength(255, ErrorMessage = "The Email must be no more than 255 characters long.")]
    [MinLength(2, ErrorMessage = "The Email must be at least 2 characters long.")]
    [Required(ErrorMessage = "The Email is required.")]
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
