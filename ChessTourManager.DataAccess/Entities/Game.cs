using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ChessTourManager.DataAccess.Entities;

public class Game
{
    private (decimal RatioSum1, decimal RatioSum2)           _prevBlackRatios;
    private (int WinsCount, int DrawsCount, int LossesCount) _prevBlackStats;

    [NotMapped]
    private (double, double) _prevPointsSum;

    [NotMapped]
    private (double, double) _prevResult;

    private (decimal RatioSum1, decimal RatioSum2) _prevWhiteRatios;

    private (int WinsCount, int DrawsCount, int LossesCount) _prevWhiteStats;

    private string? _result;

    public Game()
    {
        UpdatePreviousValues();
    }

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
                return _result ??= WhitePoints + " – " + BlackPoints;
            }

            if (Math.Abs(WhitePoints - 1) < 0.0001)
            {
                return _result ??= "+ – -";
            }

            if (Math.Abs(BlackPoints - 1) < 0.0001)
            {
                return _result ??= "- – +";
            }

            return _result ??= "0 – 0";
        }
        set
        {
            _result = value;
            if (_result.Equals(_prevResult.Item1 + " – " + _prevResult.Item2))
            {
                return;
            }

            // Restore old values.
            RestoreOldValues();

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

            UpdatePreviousValues();
        }
    }

    private void UpdatePreviousValues()
    {
        if (PlayerWhite is null || PlayerBlack is null)
        {
            return;
        }

        _prevResult = (WhitePoints, BlackPoints);

        _prevPointsSum = (PlayerWhite.PointsCount, PlayerBlack.PointsCount);

        _prevWhiteStats  = (PlayerWhite.WinsCount, PlayerWhite.DrawsCount, PlayerWhite.LossesCount);
        _prevWhiteRatios = (PlayerWhite.RatioSum1, PlayerWhite.RatioSum2);

        _prevBlackStats  = (PlayerBlack.WinsCount, PlayerBlack.DrawsCount, PlayerBlack.LossesCount);
        _prevBlackRatios = (PlayerBlack.RatioSum1, PlayerBlack.RatioSum2);
    }

    private void RestoreOldValues()
    {
        (PlayerWhite.PointsCount, PlayerBlack.PointsCount) = _prevPointsSum;

        (PlayerWhite.WinsCount, PlayerWhite.DrawsCount, PlayerWhite.LossesCount) = _prevWhiteStats;
        (PlayerWhite.RatioSum1, PlayerWhite.RatioSum2)                           = _prevWhiteRatios;

        (PlayerBlack.WinsCount, PlayerBlack.DrawsCount, PlayerBlack.LossesCount) = _prevBlackStats;
        (PlayerBlack.RatioSum1, PlayerBlack.RatioSum2)                           = _prevBlackRatios;
    }
}
