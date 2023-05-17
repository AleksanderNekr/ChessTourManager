using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTourManager.DataAccess.Entities;

public class System
{
    public int Id { get; set; }

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
