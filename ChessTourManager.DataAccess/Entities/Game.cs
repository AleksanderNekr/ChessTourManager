using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ChessTourManager.DataAccess.Entities;

public class Game
{
    private string? _result;

    public int WhiteId { get; set; }

    public int BlackId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public int TourNumber { get; set; }

    public double WhitePoints { get; set; }

    public double BlackPoints { get; set; }

    public bool IsPlayed { get; set; }

    public Player PlayerBlack { get; set; } = null!;

    public Player PlayerWhite { get; set; } = null!;

    [NotMapped]
    public string Result
    {
        get
        {
            if (IsPlayed)
            {
                return _result ??= WhitePoints.ToString(CultureInfo.InvariantCulture) + " – "
                                 + BlackPoints.ToString(CultureInfo.InvariantCulture);
            }

            if (Math.Abs(WhitePoints - 1) < 0.0001)
            {
                return _result ??= "+ – -";
            }

            if (Math.Abs(BlackPoints - 1) < 0.0001)
            {
                return _result ??= "- – +";
            }

            return _result ??= "–";
        }
        set
        {
            _result = value;

            if (value is "–" or "0 – 0")
            {
                SetResult(0, 0);
                IsPlayed = false;
                return;
            }

            string[] res = value.Split(" – ");
            switch (res[0])
            {
                case "+":
                    SetResult(1, 0);
                    IsPlayed = false;
                    break;
                case "-":
                    SetResult(0, 1);
                    IsPlayed = false;
                    break;
                default:
                    SetResult(double.Parse(res[0], CultureInfo.InvariantCulture),
                              double.Parse(res[1], CultureInfo.InvariantCulture));
                    IsPlayed = true;
                    break;
            }
        }
    }

    private void SetResult(double whitePoints, double blackPoints)
    {
        PlayerWhite.PointsAmount -= WhitePoints;
        PlayerBlack.PointsAmount -= BlackPoints;

        WhitePoints = whitePoints;
        BlackPoints = blackPoints;

        PlayerWhite.PointsAmount += WhitePoints;
        PlayerBlack.PointsAmount += BlackPoints;
    }
}
