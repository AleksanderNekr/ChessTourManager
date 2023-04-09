namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public partial class CreateTournamentWindow
{
    public CreateTournamentWindow()
    {
        InitializeComponent();
        DatePicker.BlackoutDates.AddDatesInPast();
        TournamentCreatedEvent.TournamentCreated += TournamentCreatedEvent_TournamentCreated;
    }

    private void TournamentCreatedEvent_TournamentCreated(object source, TournamentCreatedEventArgs tournamentCreatedEventArgs)
    {
        Close();
    }
}
