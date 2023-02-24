using System.Collections.Generic;

namespace ChessTourManagerWpf.Models.Entities;

public class Kind
{
    public int KindId { get; set; }

    public string KindName { get; set; } = null!;

    public virtual ICollection<Tournament> Tournaments { get; } = new List<Tournament>();
}
