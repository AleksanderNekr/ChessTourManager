﻿using System;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageRatings;

public partial class RatingGridControl : IDisposable
{
    public RatingGridControl()
    {
        InitializeComponent();
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    private void TournamentOpenedEvent_TournamentOpened(object source, TournamentOpenedEventArgs e)
    {
        TeamColumn.Visibility = e.OpenedTournament.Kind.KindName == "single"
                                    ? Visibility.Collapsed
                                    : Visibility.Visible;
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    public void Dispose()
    {
        TournamentOpenedEvent.TournamentOpened -= TournamentOpenedEvent_TournamentOpened;
        ((IDisposable)DataContext).Dispose();
    }
}
