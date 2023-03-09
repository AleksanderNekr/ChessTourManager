using System;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public partial class PlayersGridControl
{
    public PlayersGridControl()
    {
        InitializeComponent();
    }

    private void DataGrid_CurrentCellChanged(object? sender, EventArgs e)
    {
        PlayersViewModel.TrySavePlayers();
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    private void DataGrid_LostFocus(object sender, RoutedEventArgs e)
    {
        PlayersViewModel.TrySavePlayers();
    }

    private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TeamChangedEvent.OnTeamChanged(new TeamChangedEventArgs(null!));
    }
}
