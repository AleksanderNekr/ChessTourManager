using System;
using System.Linq;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.DataAccess.Queries.Delete;

internal class DeleteQuery : IDeleteQueries
{
    private static ChessTourContext _context = new();

    public DeleteQuery(ChessTourContext context)
    {
        _context = context;
    }

    public DeleteResult TryDeletePlayer(Player player)
    {
        try
        {
            if (CheckIfInGames(player))
            {
                return DeleteResult.Failed;
            }

            _context.Players.Remove(player);
            _context.SaveChanges();

            return DeleteResult.Success;
        }
        catch (Exception e)
        {
            _context.Entry(player).State = EntityState.Unchanged;

            return DeleteResult.Failed;
        }
    }

    public DeleteResult TryDeleteTournament(Tournament? tournament)
    {
        try
        {
            // Drop cascade.
            _context.Games
                    .Where(g => g              != null
                             && g.OrganizerId  == tournament.OrganizerId
                             && g.TournamentId == tournament.Id)
                    .ToList()
                    .ForEach(g => _context.Games.Remove(g));

            _context.Tournaments.Remove(tournament);
            _context.SaveChanges();
            return DeleteResult.Success;
        }
        catch (DbUpdateException e)
        {
            _context.Entry(tournament).State = EntityState.Unchanged;
            return DeleteResult.Failed;
        }
        catch (Exception e)
        {
            return DeleteResult.Failed;
        }
    }

    private static bool CheckIfInGames(Player player)
    {
        bool isInWhiteGames = player.GamesWhiteOpponents.Count != 0;
        bool isInBlackGames = player.GamesBlackOpponents.Count != 0;
        return isInWhiteGames || isInBlackGames;
    }
}
