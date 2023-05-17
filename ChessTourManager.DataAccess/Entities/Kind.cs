using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTourManager.DataAccess.Entities;

public class Kind
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public IEnumerable<Tournament> Tournaments { get; } = new List<Tournament>();

    [NotMapped]
    public string KindNameLocalized
    {
        get
        {
            return this.Name switch
                   {
                       "single"      => "Single",
                       "team"        => "Team",
                       "single-team" => "Single-team",
                       _             => "Unknown"
                   };
        }
    }

    public override string ToString()
    {
        return this.KindNameLocalized;
    }
}
