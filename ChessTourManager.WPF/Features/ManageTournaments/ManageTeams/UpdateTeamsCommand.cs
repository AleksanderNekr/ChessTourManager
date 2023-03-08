using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams;

public class UpdateTeamsCommand : CommandBase
{
    private readonly ManageTeamsViewModel _manageTeamsViewModel;

    public UpdateTeamsCommand(ManageTeamsViewModel manageTeamsViewModel)
    {
        _manageTeamsViewModel = manageTeamsViewModel;
    }

    public override void Execute(object? parameter)
    {
        _manageTeamsViewModel.UpdateTeams();
    }
}
