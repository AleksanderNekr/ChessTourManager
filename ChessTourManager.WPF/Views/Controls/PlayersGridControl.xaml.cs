using System;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.ViewModels;

namespace ChessTourManager.WPF.Views.Controls;

public partial class PlayersGridControl
{
    public PlayersGridControl() => InitializeComponent();

    private void DataGrid_CurrentCellChanged(object? sender, EventArgs e) =>
        PlayersViewModel.PlayersContext.SaveChanges();

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e) => e.Row.Header = e.Row.GetIndex() + 1;

    private void DataGrid_LostFocus(object sender, RoutedEventArgs e)
    {
        PlayersViewModel.PlayersContext.SaveChanges();
    }
}
