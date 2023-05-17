using System;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;

public partial class TeamsListControl : IDisposable
{
    public TeamsListControl()
    {
        this.InitializeComponent();
        TeamChangedEvent.TeamEdited += this.TeamEditedEventTeamEdited;
    }

    private void TeamEditedEventTeamEdited(object source, TeamChangedEventArgs teamChangedEventArgs)
    {
        // Update tree view
        this.TreeView.Items.Refresh();
    }

    public void Dispose()
    {
        TeamChangedEvent.TeamEdited -= this.TeamEditedEventTeamEdited;
        ((IDisposable)this.DataContext).Dispose();
    }
}
