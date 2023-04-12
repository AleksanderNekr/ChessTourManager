using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTourManager.DataAccess.Entities;

public class Kind
{
    public int KindId { get; set; }

    public string KindName { get; set; } = null!;

    public IEnumerable<Tournament> Tournaments { get; } = new List<Tournament>();

    [NotMapped]
    public string KindNameLocalized
    {
        get
        {
            return KindName switch
                   {
                       "single"      => "Личный",
                       "team"        => "Командный",
                       "single-team" => "Лично-командный",
                       _             => "Неизвестный"
                   };
        }
    }
}
