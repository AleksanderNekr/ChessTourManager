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
}
