using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;
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

    private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TeamChangedEvent.OnTeamChanged(new TeamChangedEventArgs(null!));
    }
}
