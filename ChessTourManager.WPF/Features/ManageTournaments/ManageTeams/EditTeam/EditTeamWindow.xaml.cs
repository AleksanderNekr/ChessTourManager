using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public partial class EditTeamWindow
{
    public EditTeamWindow()
    {
        this.InitializeComponent();
    }


    public EditTeamWindow(Team? team)
    {
        this.InitializeComponent();
        this.DataContext = new EditTeamViewModel(team);
    }
}
