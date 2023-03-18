using System;
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
            _context.Players.Remove(player);
            _context.SaveChanges();
            return DeleteResult.Success;
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Нельзя удалить игрока, который участвует в игре!", 
                            "Ошибка при удалении игрока", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
            _context.Entry(player).State = EntityState.Unchanged;
            return DeleteResult.Failed;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка при удалении игрока",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            _context.Entry(player).State = EntityState.Unchanged;
            return DeleteResult.Failed;
        }
    }

    public DeleteResult TryDeleteTournament(Tournament tournament)
    {
        try
        {
            _context.Tournaments.Remove(tournament);
            _context.SaveChanges();
            return DeleteResult.Success;
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Нельзя удалить данный турнир!", "Ошибка при удалении турнира",
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
}
