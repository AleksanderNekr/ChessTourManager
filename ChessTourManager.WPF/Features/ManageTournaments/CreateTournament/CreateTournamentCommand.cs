using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public class CreateTournamentCommand : CommandBase
{
    private readonly CreateTournamentViewModel _createTournamentViewModel;

    public CreateTournamentCommand(CreateTournamentViewModel createTournamentViewModel)
    {
        _createTournamentViewModel = createTournamentViewModel;
    }

    public override void Execute(object? parameter)
    {
        // Send insert query to database.
    }
}
