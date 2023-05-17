using System.Collections.Generic;

namespace ChessTourManager.DataAccess.Entities;

public class Ratio
{
    public int RatioId { get; set; }

    public string RatioName { get; set; } = null!;

    public virtual ICollection<Tournament> Tournaments { get; } = new List<Tournament>();
}
