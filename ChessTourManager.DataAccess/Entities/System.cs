using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTourManager.DataAccess.Entities;

public class System
{
    public int SystemId { get; set; }

    public string SystemName { get; set; } = null!;

    public IEnumerable<Tournament> Tournaments { get; } = new List<Tournament>();

    [NotMapped]
    public string SystemNameLocalized
    {
        get
        {
            return this.SystemName switch
                   {
                       "swiss"       => "Швейцарская",
                       "round-robin" => "Круговая",
                       _             => "Неизвестная"
                   };
        }
    }
}
