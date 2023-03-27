using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public partial class EditTeamWindow
{
    public EditTeamWindow()
    {
        InitializeComponent();
    }


    public EditTeamWindow(Team? team)
    {
        InitializeComponent();
        DataContext = new EditTeamViewModel(team);
    }
}
