namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;

public partial class AddTeamWindow
{
    public AddTeamWindow()
    {
        this.InitializeComponent();
        TeamAddedEvent.TeamAdded += this.TeamAddedEvent_TeamAdded;
    }

    private void TeamAddedEvent_TeamAdded(object source, TeamAddedEventArgs teamAddedEventArgs)
    {
        TeamAddedEvent.TeamAdded -= this.TeamAddedEvent_TeamAdded;
        this.Close();
    }
}
