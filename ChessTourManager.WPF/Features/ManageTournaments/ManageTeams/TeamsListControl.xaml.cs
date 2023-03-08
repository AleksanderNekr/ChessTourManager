using System.Windows;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;

public partial class TeamsListControl : UserControl
{
    public TeamsListControl()
    {
        InitializeComponent();
        TeamChangedEvent.TeamChanged += TeamChangedEvent_TeamChanged;
    }

    private void TeamChangedEvent_TeamChanged(TeamChangedEventArgs e)
    {
        // Update tree view
        TreeView.Items.Refresh();
    }
}

