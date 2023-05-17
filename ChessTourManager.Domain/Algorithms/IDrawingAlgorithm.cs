﻿using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Algorithms;

public interface IDrawingAlgorithm
{
    public static IDrawingAlgorithm Initialize(ChessTourContext context, Tournament tournament) =>
        new RoundRobin(context, tournament);

    public IList<(int, int)> StartNewTour(int currentTour);

    public int NewTourNumber { get; }

    static IDrawingAlgorithm? Initialize(ChessTourContext context, int tournamentId)
    {
        Tournament? tournament = context.Tournaments.Single(t => t != null && t.Id == tournamentId);
        return tournament is null
                   ? null
                   : Initialize(context, tournament);
    }
}
