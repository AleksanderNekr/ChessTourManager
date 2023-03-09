using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;

public class AddTeamCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        new AddTeamWindow().ShowDialog();
    }
}
