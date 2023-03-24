using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ChessTourManager.Web.Models;

public class LoginViewModel
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
