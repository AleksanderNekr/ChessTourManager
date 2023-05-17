using System;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public partial class PlayersGridControl : IDisposable
{
    public PlayersGridControl()
    {
        this.InitializeComponent();
        TournamentOpenedEvent.TournamentOpened += this.TournamentOpenedEvent_TournamentOpened;
    }

    private void TournamentOpenedEvent_TournamentOpened(object source, TournamentOpenedEventArgs e)
    {
        this.TeamColumn.Visibility = e.OpenedTournament.Kind.KindName == "single"
                                         ? Visibility.Collapsed
                                         : Visibility.Visible;
        ((MainWindow)Window.GetWindow(this)).TeamTab.Visibility = e.OpenedTournament.Kind.KindName == "single"
                                                                      ? Visibility.Collapsed
                                                                      : Visibility.Visible;
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    public void Dispose()
    {
        TournamentOpenedEvent.TournamentOpened -= this.TournamentOpenedEvent_TournamentOpened;
        ((IDisposable)this.DataContext).Dispose();
    }
}
