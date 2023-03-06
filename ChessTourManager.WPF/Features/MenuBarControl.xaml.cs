using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

namespace ChessTourManager.WPF.Features;

public partial class MenuBarControl : UserControl
{
    public MenuBarControl()
    {
        InitializeComponent();
    }

    private void CreateTournamentMenuItem_Click(object sender, RoutedEventArgs e)
    {
        new CreateTournamentWindow().ShowDialog();
    }
}
