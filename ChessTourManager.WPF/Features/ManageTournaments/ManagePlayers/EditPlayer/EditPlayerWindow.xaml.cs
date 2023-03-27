using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;

public partial class EditPlayerWindow
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
