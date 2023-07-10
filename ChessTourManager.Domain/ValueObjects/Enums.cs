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

public enum GameResult
{
    WhiteWin,
    BlackWin,
    Draw,
    WhiteWinByDefault,
    BlackWinByDefault,
    NotPlayed,
}

public enum PlayerColor
{
    White,
    Black,
}
