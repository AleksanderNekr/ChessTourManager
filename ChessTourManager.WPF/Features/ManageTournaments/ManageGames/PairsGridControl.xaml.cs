using System;
using System.Windows.Controls;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public partial class PairsGridControl : UserControl
{
    public PairsGridControl()
    {
        InitializeComponent();
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    private void DataGrid_CurrentCellChanged(object? sender, EventArgs e)
    {
        PairsGridViewModel.PairsContext.SaveChanges();

        ResultChangedEvent.OnResultChanged(sender, new ResultChangedEventArgs(DataGrid.CurrentCell.Item as Game));
    }
}
