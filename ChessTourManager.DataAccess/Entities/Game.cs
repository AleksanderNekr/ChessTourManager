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
            if (this.IsPlayed)
            {
                return this._result ??= this.WhitePoints.ToString(CultureInfo.InvariantCulture) + " – "
                                                                                             + this.BlackPoints.ToString(CultureInfo.InvariantCulture);
            }

            if (Math.Abs(this.WhitePoints - 1) < 0.0001)
            {
                return this._result ??= "+ – -";
            }

            if (Math.Abs(this.BlackPoints - 1) < 0.0001)
            {
                return this._result ??= "- – +";
            }

            return this._result ??= "–";
        }
        set
        {
            this._result = value;

            if (value is "–" or "0 – 0")
            {
                this.SetResult(0, 0);
                this.IsPlayed = false;
                return;
            }

            string[] res = value.Split(" – ");
            switch (res[0])
            {
                case "+":
                    this.SetResult(1, 0);
                    this.IsPlayed = false;
                    break;
                case "-":
                    this.SetResult(0, 1);
                    this.IsPlayed = false;
                    break;
                default:
                    this.SetResult(double.Parse(res[0], CultureInfo.InvariantCulture),
                                   double.Parse(res[1], CultureInfo.InvariantCulture));
                    this.IsPlayed = true;
                    break;
            }
        }
    }

    private void SetResult(double whitePoints, double blackPoints)
    {
        this.PlayerWhite.PointsAmount -= this.WhitePoints;
        this.PlayerBlack.PointsAmount -= this.BlackPoints;

        this.WhitePoints = whitePoints;
        this.BlackPoints    = blackPoints;

        this.PlayerWhite.PointsAmount += this.WhitePoints;
        this.PlayerBlack.PointsAmount += this.BlackPoints;
    }
}
