using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;

public class AddTeamCommand : CommandBase
{
    private readonly ManageTeamsViewModel _manageTeamsViewModel;

    public AddTeamCommand(ManageTeamsViewModel manageTeamsViewModel)
    {
        _manageTeamsViewModel = manageTeamsViewModel;
    }

    public override void Execute(object? parameter)
    {
        new AddTeamWindow().ShowDialog();
    }
}
