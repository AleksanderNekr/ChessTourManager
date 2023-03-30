using System;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Helpers;
using ChessTourManager.WPF.Helpers.FileHelpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageRatings;

public class PrintRatingListCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is not DataGrid dataGrid)
        {
            return;
        }

        try
        {
            if (PrintMethods.TryPrintFrameworkElement(dataGrid))
            {
                MessageBox.Show("Данные успешно отправлены на печать", "Печать рейтинг-листа",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Печать рейтинг-листа",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
