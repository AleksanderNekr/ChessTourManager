namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;

public partial class AddTeamWindow
{
    public AddTeamWindow()
    {
        InitializeComponent();
        TeamAddedEvent.TeamAdded += TeamAddedEvent_TeamAdded;
    }

    private void TeamAddedEvent_TeamAdded(TeamAddedEventArgs e)
    {
        Close();
    }
}
