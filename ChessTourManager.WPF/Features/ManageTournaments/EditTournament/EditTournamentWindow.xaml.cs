using System.Windows;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public partial class EditTournamentWindow : Window
{
    public EditTournamentWindow()
    {
        InitializeComponent();
        DatePicker.BlackoutDates.AddDatesInPast();
        TournamentEditedEvent.TournamentEdited += TournamentEditedEvent_TournamentEdited;
    }

    public EditTournamentWindow(object? parameter)
    {
        EditTournamentViewModel.EditingTournament = parameter as Tournament;
        InitializeComponent();
        TournamentEditedEvent.TournamentEdited += TournamentEditedEvent_TournamentEdited;
    }

    private void TournamentEditedEvent_TournamentEdited(TournamentEditedEventArgs e)
    {
        Close();
    }
}
