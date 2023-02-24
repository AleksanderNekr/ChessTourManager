using System.Windows;
using System.Windows.Controls;
using ChessTourManagerWpf.Views;

namespace ChessTourManagerWpf.Controls;

public partial class MenuBarControl : UserControl
{
    public MenuBarControl() => InitializeComponent();

    private void CreateTournamentMenuItem_Click(object sender, RoutedEventArgs e)
    {
        new CreateTournamentWindow().ShowDialog();
    }
}
