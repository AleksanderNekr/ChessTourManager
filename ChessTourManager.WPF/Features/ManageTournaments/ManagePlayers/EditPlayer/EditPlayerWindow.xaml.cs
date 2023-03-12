using System.Windows;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;

public partial class EditPlayerWindow : Window
{
    public EditPlayerWindow()
    {
        InitializeComponent();
    }


    public EditPlayerWindow(Player? player)
    {
        InitializeComponent();
        DataContext = new EditPlayerViewModel(player);
    }
}

