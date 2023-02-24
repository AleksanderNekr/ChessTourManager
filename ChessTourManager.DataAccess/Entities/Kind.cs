using System.Collections.Generic;

namespace ChessTourManager.DataAccess.Entities;

public class Kind
{
    public int KindId { get; set; }

    public string KindName { get; set; } = null!;

    public virtual ICollection<Tournament> Tournaments { get; } = new List<Tournament>();
}
