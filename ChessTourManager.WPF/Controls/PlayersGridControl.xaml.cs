﻿using System;
using ChessTourManager.WPF.ViewModels;

namespace ChessTourManager.WPF.Controls;

public partial class PlayersGridControl
{
    public PlayersGridControl() => InitializeComponent();

    private void DataGrid_CurrentCellChanged(object? sender, EventArgs e)
    {
        PlayersViewModel.PlayersContext.SaveChanges();
    }
}
