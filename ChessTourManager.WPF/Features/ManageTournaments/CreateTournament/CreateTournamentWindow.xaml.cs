using System.Windows.Controls;

namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public partial class CreateTournamentWindow
{
    public CreateTournamentWindow()
    {
        InitializeComponent();
        DatePicker.BlackoutDates.AddDatesInPast();
        TournamentCreatedEvent.TournamentCreated += TournamentCreatedEvent_TournamentCreated;
    }

    private void TournamentCreatedEvent_TournamentCreated(TournamentCreatedEventArgs e) => Close();
}
