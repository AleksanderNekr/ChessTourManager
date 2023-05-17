using System;
using System.Windows.Controls;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public partial class PairsGridControl : IDisposable
{
    public PairsGridControl()
    {
        this.InitializeComponent();
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    private void DataGrid_CurrentCellChanged(object? sender, EventArgs e)
    {
        PlayersViewModel.PlayersContext.SaveChanges();

        ResultChangedEvent.OnResultChanged(sender, new ResultChangedEventArgs(this.DataGrid.CurrentCell.Item as Game));
    }

    public void Dispose()
    {
        ((IDisposable)this.DataContext).Dispose();
    }
}
