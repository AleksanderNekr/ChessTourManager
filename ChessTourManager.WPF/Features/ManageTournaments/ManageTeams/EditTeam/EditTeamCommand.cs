using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public class EditTeamCommand : CommandBase
{
    private readonly ManageTeamsViewModel _manageTeamsViewModel;

    public EditTeamCommand(ManageTeamsViewModel manageTeamsViewModel)
    {
        _manageTeamsViewModel = manageTeamsViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is not Team team)
        {
            return;
        }

        EditTeamWindow editTeamWindow = new(team);
        editTeamWindow.ShowDialog();
    }
}
