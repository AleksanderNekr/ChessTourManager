using System;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public partial class PlayersGridControl
{
    public PlayersGridControl()
    {
        InitializeComponent();
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        TeamColumn.Visibility = e.OpenedTournament.Kind.KindName == "single"
                                    ? Visibility.Collapsed
                                    : Visibility.Visible;
        ((MainWindow)Window.GetWindow(this)).TeamTab.Visibility = e.OpenedTournament.Kind.KindName == "single"
                                                                      ? Visibility.Collapsed
                                                                      : Visibility.Visible;
    }

    private void DataGrid_CurrentCellChanged(object? sender, EventArgs e)
    {
        if (!PlayersViewModel.TrySavePlayers())
        {
            ((PlayersViewModel)DataContext).UpdatePlayers();
            ((PlayersViewModel)DataContext).UpdateGroups();
            ((PlayersViewModel)DataContext).UpdateTeams();
        }
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    private void DataGrid_LostFocus(object sender, RoutedEventArgs e)
    {
        if (!PlayersViewModel.TrySavePlayers())
        {
            ((PlayersViewModel)DataContext).UpdatePlayers();
            ((PlayersViewModel)DataContext).UpdateGroups();
            ((PlayersViewModel)DataContext).UpdateTeams();
        }
    }

    private void Team_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!PlayersViewModel.TrySavePlayers())
        {
            ((PlayersViewModel)DataContext).UpdatePlayers();
            ((PlayersViewModel)DataContext).UpdateGroups();
            ((PlayersViewModel)DataContext).UpdateTeams();
        }
        else
        {
            TeamEditedEvent.OnTeamChanged(new TeamChangedEventArgs(null));
        }
    }

    private void Group_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!PlayersViewModel.TrySavePlayers())
        {
            ((PlayersViewModel)DataContext).UpdatePlayers();
            ((PlayersViewModel)DataContext).UpdateGroups();
            ((PlayersViewModel)DataContext).UpdateTeams();
        }
        else
        {
            GroupChangedEvent.OnGroupChanged(new GroupChangedEventArgs(null));
        }
    }
}
