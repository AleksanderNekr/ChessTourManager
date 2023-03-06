using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public partial class PlayersGridControl
{
    public PlayersGridControl()
    {
        InitializeComponent();
    }

    private void DataGrid_CurrentCellChanged(object? sender, EventArgs e)
    {
        TryToSave();
    }

    private static void TryToSave()
    {
        try
        {
            PlayersViewModel.PlayersContext.SaveChanges();
        }
        catch (ConstraintException)
        {
            MessageBox.Show("Игрок с такими параметрами уже существует!", "Ошибка изменения игрока",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.InnerException?.Message ?? exception.Message, "Ошибка изменения игрока",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    private void DataGrid_LostFocus(object sender, RoutedEventArgs e)
    {
        TryToSave();
    }
}
