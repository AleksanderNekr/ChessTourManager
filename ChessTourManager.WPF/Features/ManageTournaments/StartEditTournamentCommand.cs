using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments;

public class StartEditTournamentCommand : CommandBase
{
    public override void Execute(object? parameter) => new EditTournamentWindow(parameter).ShowDialog();
}
