namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public partial class CreateTournamentWindow
{
    public CreateTournamentWindow()
    {
        this.InitializeComponent();
        this.DatePicker.BlackoutDates.AddDatesInPast();
        TournamentCreatedEvent.TournamentCreated += this.TournamentCreatedEvent_TournamentCreated;
    }

    private void TournamentCreatedEvent_TournamentCreated(object source, TournamentCreatedEventArgs tournamentCreatedEventArgs)
    {
        TournamentCreatedEvent.TournamentCreated -= this.TournamentCreatedEvent_TournamentCreated;
        this.Close();
    }
}
