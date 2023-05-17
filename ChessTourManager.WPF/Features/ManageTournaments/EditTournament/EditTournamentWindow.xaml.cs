using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public partial class EditTournamentWindow
{
    public EditTournamentWindow()
    {
        this.InitializeComponent();
        this.DatePicker.BlackoutDates.AddDatesInPast();
        TournamentEditedEvent.TournamentEdited += this.TournamentEditedEvent_TournamentEdited;
    }

    public EditTournamentWindow(object? parameter)
    {
        EditTournamentViewModel.EditingTournament = parameter as Tournament;
        this.InitializeComponent();
        TournamentEditedEvent.TournamentEdited += this.TournamentEditedEvent_TournamentEdited;
    }

    private void TournamentEditedEvent_TournamentEdited(object source, TournamentEditedEventArgs tournamentEditedEventArgs)
    {
        TournamentEditedEvent.TournamentEdited -= this.TournamentEditedEvent_TournamentEdited;
        this.Close();
    }
}
