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
                MessageBox.Show("Нельзя удалить игрока, который участвует в игре!",
                                "Ошибка при удалении игрока",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return DeleteResult.Failed;
            }

            _context.Players.Remove(player);
            _context.SaveChanges();

            return DeleteResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при удалении игрока",
                            MessageBoxButton.OK, MessageBoxImage.Error);
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
                             && g.TournamentId == tournament.TournamentId)
                    .ToList()
                    .ForEach(g => _context.Games.Remove(g));

            _context.Tournaments.Remove(tournament);
            _context.SaveChanges();
            return DeleteResult.Success;
        }
        catch (DbUpdateException e)
        {
            MessageBox.Show($"Нельзя удалить данный турнир!\n{e.InnerException?.Message}",
                            "Ошибка при удалении турнира",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            _context.Entry(tournament).State = EntityState.Unchanged;
            return DeleteResult.Failed;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка при удалении турнира", MessageBoxButton.OK, MessageBoxImage.Error);
            _context.Entry(tournament).State = EntityState.Unchanged;
            return DeleteResult.Failed;
        }
    }

    private static bool CheckIfInGames(Player player)
    {
        bool isInWhiteGames = player.WhiteGamePlayers.Count != 0;
        bool isInBlackGames = player.BlackGamePlayers.Count != 0;
        return isInWhiteGames || isInBlackGames;
    }
}
