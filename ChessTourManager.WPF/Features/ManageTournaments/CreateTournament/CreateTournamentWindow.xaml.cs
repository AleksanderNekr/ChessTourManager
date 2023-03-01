namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public partial class CreateTournamentWindow
{
    public CreateTournamentWindow()
    {
        InitializeComponent();
        DatePicker.BlackoutDates.AddDatesInPast();
    }
}
