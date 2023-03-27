using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public partial class AddPlayerWindow
{
    public AddPlayerWindow()
    {
        InitializeComponent();
    }

    public AddPlayerWindow(Team? team)
    {
        InitializeComponent();
        DataContext                  =  new AddPlayerViewModel(team);
        PlayerAddedEvent.PlayerAdded += PlayerAddedEvent_PlayerAdded;
    }

    private void PlayerAddedEvent_PlayerAdded(PlayerAddedEventArgs e)
    {
        Close();
    }
}
