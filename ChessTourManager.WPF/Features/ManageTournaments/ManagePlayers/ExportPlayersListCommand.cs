using System;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Helpers;
using ChessTourManager.WPF.Helpers.FileHelpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public class ExportPlayersListCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is not DataGrid dataGrid)
        {
            return;
        }

        try
        {
            if (ExportTableMethods.TryExportGrid(dataGrid, skipLastColumnsCount: 1))
            {
                MessageBox.Show("Данные успешно экспортированы", "Экспорт списка участников",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Экспорт списка участников",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
