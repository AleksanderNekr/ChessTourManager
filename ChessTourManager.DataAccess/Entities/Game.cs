using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ChessTourManager.DataAccess.Entities;

public class Game
{
    private string? _result;

    public Game()
    {
        UpdateGame();
    }


    public int WhiteId { get; set; }

    public int BlackId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public int TourNumber { get; set; }

    public double WhitePoints { get; set; }

    public double BlackPoints { get; set; }

    public bool IsPlayed { get; set; }

    public Player PlayerBlack { get; set; }

    public Player PlayerWhite { get; set; }

    [NotMapped]
    public string Result
    {
        get { return _result ??= WhitePoints + " – " + BlackPoints; }
        set
        {
            _result = value;
            string[] res = value.Split(" – ");
            switch (res[0])
            {
                case "0" when res[1] == "0":
                    WhitePoints = double.Parse(res[0], CultureInfo.InvariantCulture);
                    BlackPoints = double.Parse(res[1], CultureInfo.InvariantCulture);
                    IsPlayed    = false;
                    break;
                case "+":
                    WhitePoints = 1;
                    BlackPoints = 0;
                    IsPlayed    = false;
                    break;
                case "-":
                    WhitePoints = 0;
                    BlackPoints = 1;
                    IsPlayed    = false;
                    break;
                default:
                    WhitePoints = double.Parse(res[0], CultureInfo.InvariantCulture);
                    BlackPoints = double.Parse(res[1], CultureInfo.InvariantCulture);
                    IsPlayed    = true;
                    break;
            }
        }
    }

    [NotMapped]
    public string WhiteLastName { get; set; }

    [NotMapped]
    public double WhitePointsCount { get; set; }

    [NotMapped]
    public string BlackLastName { get; set; }

    [NotMapped]
    public double BlackPointsCount { get; set; }

    public void UpdateGame()
    {
        if (PlayerWhite is null || PlayerBlack is null)
        {
            return;
        }

        WhiteLastName    = PlayerWhite.PlayerLastName;
        WhitePointsCount = PlayerWhite.PointsCount;
        BlackLastName    = PlayerBlack.PlayerLastName;
        BlackPointsCount = PlayerBlack.PointsCount;
    }
}
