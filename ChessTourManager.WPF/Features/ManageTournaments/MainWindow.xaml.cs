using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Features.ManageTournaments;

/// <inheritdoc cref="System.Windows.Window" />
public partial class MainWindow
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        ((IDisposable)this.PairsGridControl).Dispose();
        ((IDisposable)this.GroupsListControl).Dispose();
        ((IDisposable)this.PlayersGridControl).Dispose();
        ((IDisposable)this.RatingGridControl).Dispose();
        ((IDisposable)this.TeamsListControl).Dispose();
        ((IDisposable)this.TreeControl).Dispose();

        ((IDisposable)this.DataContext).Dispose();
    }
}
