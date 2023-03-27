using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;

public partial class TeamsListControl
{
    public TeamsListControl()
    {
        InitializeComponent();
        TeamEditedEvent.TeamEdited += TeamEditedEventTeamEdited;
    }

    private void TeamEditedEventTeamEdited(TeamChangedEventArgs e)
    {
        // Update tree view
        TreeView.Items.Refresh();
    }
}
