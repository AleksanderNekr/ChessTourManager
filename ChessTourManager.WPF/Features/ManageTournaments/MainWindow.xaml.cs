using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Features.ManageTournaments;

/// <inheritdoc cref="System.Windows.Window" />
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        ((IDisposable)PairsGridControl).Dispose();
        ((IDisposable)GroupsListControl).Dispose();
        ((IDisposable)PlayersGridControl).Dispose();
        ((IDisposable)RatingGridControl).Dispose();
        ((IDisposable)TeamsListControl).Dispose();
        ((IDisposable)TreeControl).Dispose();

        ((IDisposable)DataContext).Dispose();
    }
}
