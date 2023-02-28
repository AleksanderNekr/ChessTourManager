﻿using System;
using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.Domain.Queries;

internal class DeleteQuery : IDeleteQueries
{
    private static ChessTourContext? _context = new();

    public DeleteQuery(ChessTourContext context) => _context = context;

    public DeleteResult TryDeletePlayer(Player player)
    {
        try
        {
            _context.Players.Remove(player);
            _context.SaveChanges();
            return DeleteResult.Success;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка при удалении игрока", MessageBoxButton.OK, MessageBoxImage.Error);
            return DeleteResult.Failed;
        }
    }
}
