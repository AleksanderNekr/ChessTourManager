using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments;
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

    private void CloseAppMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ChangeUser_OnClick(object sender, RoutedEventArgs e)
    {
        new AuthWindow().Show();
        // Close the host of this control
        Window.GetWindow(this)?.Close();
    }
}
