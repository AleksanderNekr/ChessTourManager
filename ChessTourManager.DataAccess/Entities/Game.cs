using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ChessTourManager.DataAccess.Entities;

public class Game
{
    private string? _result;

    [DisplayName("Player for white")]
    public int WhiteId { get; set; }

    [DisplayName("Player for black")]
    public int BlackId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    [DisplayName("Tour number")]
    public int TourNumber { get; set; }

    [DisplayName("Points for white")]
    public double WhitePoints { get; set; }

    [DisplayName("Points for black")]
    public double BlackPoints { get; set; }

    [DisplayName("Is played")]
    public bool IsPlayed { get; set; }

    [DisplayName("Player for black")]
    public Player? PlayerBlack { get; set; } = null!;

    [DisplayName("Player for white")]
    public Player? PlayerWhite { get; set; } = null!;

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
        this.RestoreCounters();

        this.WhitePoints = whitePoints;
        this.BlackPoints = blackPoints;

        this.PlayerWhite.PointsAmount += this.WhitePoints;
        this.PlayerBlack.PointsAmount += this.BlackPoints;
        this.UpdateCounters(whitePoints, blackPoints);
    }

    private void UpdateCounters(double whitePoints, double blackPoints)
    {
        if (whitePoints == 0 && blackPoints == 0)
        {
            return;
        }

        if (whitePoints == 0)
        {
            this.PlayerWhite.LossesCount++;
        }
        else if (Math.Abs(whitePoints - 1) < 0.00001)
        {
            this.PlayerWhite.WinsCount++;
        }
        else
        {
            this.PlayerWhite.DrawsCount++;
        }

        if (blackPoints == 0)
        {
            this.PlayerBlack.LossesCount++;
        }
        else if (Math.Abs(blackPoints - 1) < 0.00001)
        {
            this.PlayerBlack.WinsCount++;
        }
        else
        {
            this.PlayerBlack.DrawsCount++;
        }
    }

    private void RestoreCounters()
    {
        if (this.WhitePoints == 0)
        {
            this.PlayerWhite.LossesCount--;
        }
        else if (Math.Abs(this.WhitePoints - 1) < 0.00001)
        {
            this.PlayerWhite.WinsCount--;
        }
        else
        {
            this.PlayerWhite.DrawsCount--;
        }

        if (this.BlackPoints == 0)
        {
            this.PlayerBlack.LossesCount--;
        }
        else if (Math.Abs(this.BlackPoints - 1) < 0.00001)
        {
            this.PlayerBlack.WinsCount--;
        }
        else
        {
            this.PlayerBlack.DrawsCount--;
        }
    }
}
