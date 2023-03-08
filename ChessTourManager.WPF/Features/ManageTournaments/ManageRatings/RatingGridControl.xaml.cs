using System.Windows.Controls;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageRatings;

public partial class RatingGridControl : UserControl
{
    public RatingGridControl()
    {
        InitializeComponent();
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }
}
