using System.Collections.Generic;

namespace ChessTourManagerWpf.Models.Entities;

public class System
{
    public int SystemId { get; set; }

    public string SystemName { get; set; } = null!;

    public virtual ICollection<Tournament> Tournaments { get; } = new List<Tournament>();
}
