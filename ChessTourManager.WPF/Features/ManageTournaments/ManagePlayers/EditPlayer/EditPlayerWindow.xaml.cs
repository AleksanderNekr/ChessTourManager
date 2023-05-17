using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;

public partial class EditPlayerWindow
{
    public EditPlayerWindow()
    {
        this.InitializeComponent();
    }


    public EditPlayerWindow(Player? player)
    {
        this.InitializeComponent();
        this.DataContext = new EditPlayerViewModel(player);
    }
}
