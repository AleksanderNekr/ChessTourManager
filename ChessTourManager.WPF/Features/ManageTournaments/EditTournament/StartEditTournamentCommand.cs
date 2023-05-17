using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public class StartEditTournamentCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        new EditTournamentWindow(parameter).ShowDialog();
    }
}
