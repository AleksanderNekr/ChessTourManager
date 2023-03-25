using System;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Helpers;
using ChessTourManager.WPF.Helpers.FileHelpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public class PrintPlayersListCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is not DataGrid dataGrid)
        {
            return;
        }

        try
        {
            if (PrintTableMethods.TryPrintDataGrid(dataGrid))
            {
                MessageBox.Show("Данные успешно отправлены на печать", "Печать списка игроков",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Печать списка игроков",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
