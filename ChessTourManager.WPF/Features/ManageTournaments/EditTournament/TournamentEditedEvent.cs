using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public class TournamentEditedEvent
{
    public delegate void TournamentEditedHandler(TournamentEditedEventArgs e);

    public static event TournamentEditedHandler? TournamentEdited;

    internal static void OnTournamentEdited(TournamentEditedEventArgs e)
    {
        TournamentEdited?.Invoke(e);
    }
}

public class TournamentEditedEventArgs : EventArgs
{
    public TournamentEditedEventArgs(Tournament newTournament)
    {
        NewTournament = newTournament;
    }

    public Tournament NewTournament { get; }
}
