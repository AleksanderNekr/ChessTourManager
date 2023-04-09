using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;

public partial class TeamsListControl
{
    public TeamsListControl()
    {
        InitializeComponent();
        TeamChangedEvent.TeamEdited += TeamEditedEventTeamEdited;
    }

    private void TeamEditedEventTeamEdited(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        // Update tree view
        TreeView.Items.Refresh();
    }
}
