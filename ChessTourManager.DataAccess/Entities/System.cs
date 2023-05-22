using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTourManager.DataAccess.Entities;

public class System
{
    public int Id { get; set; }

    [MinLength(2, ErrorMessage = "The system name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "The system name must be no more than 50 characters long.")]
    [Required]
    [RegularExpression(@"^[A-Z][a-z]+$",
                       ErrorMessage = "The system name must start with a capital letter and contain only letters.")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = null!;

    public IEnumerable<Tournament> Tournaments { get; } = new List<Tournament>();

    [NotMapped]
    public string SystemNameLocalized
    {
        get
        {
            return this.Name switch
                   {
                       "swiss"       => "Swiss",
                       "round-robin" => "Round-Robin",
                       _             => "Unknown"
                   };
        }
    }
}
