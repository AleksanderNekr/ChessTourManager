namespace ChessTourManager.Domain.ValueObjects;

public enum Kind
{
    Single,
    Team,
    SingleTeam,
}

public enum Coefficient
{
    Berger,
    SimpleBerger,
    Buchholz,
    TotalBuchholz,
}

public enum DrawSystem
{
    RoundRobin,
    Swiss,
}
