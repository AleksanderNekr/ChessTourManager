using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public static class TournamentEditedEvent
{
    public delegate void TournamentEditedHandler(object source, TournamentEditedEventArgs e);

    public static event TournamentEditedHandler? TournamentEdited;

    internal static void OnTournamentEdited(object source, TournamentEditedEventArgs e)
    {
        TournamentEdited?.Invoke(source, e);
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
