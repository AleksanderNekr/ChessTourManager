namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;

public partial class AddTeamWindow
{
    public AddTeamWindow()
    {
        InitializeComponent();
        TeamAddedEvent.TeamAdded += TeamAddedEvent_TeamAdded;
    }

    private void TeamAddedEvent_TeamAdded(object source, TeamAddedEventArgs teamAddedEventArgs)
    {
        TeamAddedEvent.TeamAdded -= TeamAddedEvent_TeamAdded;
        Close();
    }
}
