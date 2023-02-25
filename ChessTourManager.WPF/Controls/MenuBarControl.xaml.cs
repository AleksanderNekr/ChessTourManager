using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Views;

namespace ChessTourManager.WPF.Controls;

public partial class MenuBarControl : UserControl
{
    public MenuBarControl() => InitializeComponent();

    private void CreateTournamentMenuItem_Click(object sender, RoutedEventArgs e) =>
        new CreateTournamentWindow().ShowDialog();
}
