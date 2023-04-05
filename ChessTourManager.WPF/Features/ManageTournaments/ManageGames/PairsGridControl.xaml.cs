using System;
using System.Windows.Controls;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public partial class PairsGridControl
{
    public PairsGridControl()
    {
        InitializeComponent();
        DataGrid.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.SizeToCells);
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
