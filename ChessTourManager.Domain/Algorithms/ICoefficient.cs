using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Algorithms;

public interface ICoefficient
{
    static ICoefficient Initialize(CoefficientType type)
    {
        return type switch
               {
                   CoefficientType.Berger        => new BergerCoefficient(),
                   CoefficientType.SimpleBerger  => new SimpleBergerCoefficient(),
                   CoefficientType.Buchholz      => new BuchholzCoefficient(),
                   CoefficientType.TotalBuchholz => new TotalBuchholzCoefficient(),
                   _                             => throw new ArgumentOutOfRangeException(nameof(type), type, null)
               };
    }

    public decimal CalculateCoefficient(Player player);
}

public enum CoefficientType
{
    /// <summary>
    /// The sum of all the points of the opponents
    /// that the contestant has beaten, plus half the sum of the points of the opponents that the contestant has drawn.
    /// </summary>
    Berger,

    /// <summary>
    /// The scores of all opponents, against whom the chess player wins, are taken with a plus sign
    /// and those against whom the chess player loses with a minus sign, the sum of which is used
    /// to calculate the best result.
    /// </summary>
    SimpleBerger,

    /// <summary>
    /// The sum of the points of each of the player's opponents.
    /// </summary>
    Buchholz,

    /// <summary>
    /// The sum of the opponents' Buchholz scores.
    /// </summary>
    TotalBuchholz,
}

public class TotalBuchholzCoefficient : ICoefficient
{
    /// <summary>
    /// The sum of the opponents' Buchholz scores.
    /// </summary>
    public decimal CalculateCoefficient(Player player)
    {
        decimal result = 0;

        foreach (Game game in player.GamesWhiteOpponents)
        {
            if (game.IsPlayed)
            {
                result += game.PlayerWhite.RatioSum1;
            }
        }

        foreach (Game game in player.GamesBlackOpponents)
        {
            if (game.IsPlayed)
            {
                result += game.PlayerBlack.RatioSum1;
            }
        }

        return result;
    }
}

public class BuchholzCoefficient : ICoefficient
{
    /// <summary>
    /// The sum of the points of each of the player's opponents.
    /// </summary>
    public decimal CalculateCoefficient(Player player)
    {
        decimal result = 0;
        foreach (Game game in player.GamesWhiteOpponents)
        {
            if (game.IsPlayed)
            {
                result += (decimal)game.PlayerWhite.PointsAmount;
            }
        }

        foreach (Game game in player.GamesBlackOpponents)
        {
            if (game.IsPlayed)
            {
                result += (decimal)game.PlayerBlack.PointsAmount;
            }
        }

        return result;
    }
}

public class SimpleBergerCoefficient : ICoefficient
{
    /// <summary>
    /// The scores of all opponents, against whom the chess player wins, are taken with a plus sign
    /// and those against whom the chess player loses with a minus sign, the sum of which is used
    /// to calculate the best result.
    /// </summary>
    public decimal CalculateCoefficient(Player player)
    {
        decimal result = 0;
        foreach (Game game in player.GamesWhiteOpponents)
        {
            if (game is { BlackPoints: 1, IsPlayed: true })
            {
                result += (decimal)game.PlayerWhite.PointsAmount;
            }
            else if (game is { BlackPoints: 0, IsPlayed: true })
            {
                result -= (decimal)game.PlayerWhite.PointsAmount;
            }
        }

        foreach (Game game in player.GamesBlackOpponents)
        {
            if (game is { WhitePoints: 1, IsPlayed: true })
            {
                result += (decimal)game.PlayerBlack.PointsAmount;
            }
            else if (game is { WhitePoints: 0, IsPlayed: true })
            {
                result -= (decimal)game.PlayerBlack.PointsAmount;
            }
        }

        return result;
    }
}

internal class BergerCoefficient : ICoefficient
{
    /// <summary>
    /// The sum of all the points of the opponents
    /// that the contestant has beaten, plus half the sum of the points of the opponents that the contestant has drawn.
    /// </summary>
    public decimal CalculateCoefficient(Player player)
    {
        var wins  = 0d;
        var draws = 0d;
        foreach (Game game in player.GamesWhiteOpponents)
        {
            if (game is { BlackPoints: 1, IsPlayed: true })
            {
                wins += game.PlayerWhite.PointsAmount;
            }
            else if (game is { BlackPoints: 0.5, IsPlayed: true })
            {
                draws += game.PlayerWhite.PointsAmount;
            }
        }

        foreach (Game game in player.GamesBlackOpponents)
        {
            if (game is { WhitePoints: 1, IsPlayed: true })
            {
                wins += game.PlayerBlack.PointsAmount;
            }
            else if (game is { WhitePoints: 0.5, IsPlayed: true })
            {
                draws += game.PlayerBlack.PointsAmount;
            }
        }

        return (decimal)(wins + draws / 2.0);
    }
}
