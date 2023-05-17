using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public partial class AddPlayerWindow
{
    public AddPlayerWindow()
    {
        this.InitializeComponent();
    }

    public AddPlayerWindow(Team? team)
    {
        this.InitializeComponent();
        this.DataContext             =  new AddPlayerViewModel(team);
        PlayerAddedEvent.PlayerAdded += this.PlayerAddedEvent_PlayerAdded;
    }

    private void PlayerAddedEvent_PlayerAdded(object source, PlayerAddedEventArgs playerAddedEventArgs)
    {
        PlayerAddedEvent.PlayerAdded -= this.PlayerAddedEvent_PlayerAdded;
        this.Close();
    }
}
