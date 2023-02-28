using System;
using System.Windows.Controls;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries;
using ChessTourManager.WPF.Commands.Events;
using ChessTourManager.WPF.ViewModels;

namespace ChessTourManager.WPF.Views.Controls;

public partial class PlayersGridControl
{
    public PlayersGridControl()
    {
        InitializeComponent();
    }

    private void DataGrid_CurrentCellChanged(object? sender, EventArgs e)
    {
        PlayersViewModel.PlayersContext.SaveChanges();
    }
}
