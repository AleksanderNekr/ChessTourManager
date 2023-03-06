using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

public class TournamentOpenedEvent
{
    /// <summary>
    ///     Delegate on handling TournamentOpenedEvent.
    /// </summary>
    public delegate void TournamentOpenedEventHandler(TournamentOpenedEventArgs e);

    public static event TournamentOpenedEventHandler? TournamentOpened;

    internal static void OnTournamentOpened(TournamentOpenedEventArgs e)
    {
        TournamentOpened?.Invoke(e);
    }
}

public class TournamentOpenedEventArgs : EventArgs
{
    public Tournament OpenedTournament;

    public TournamentOpenedEventArgs(Tournament openedTournament)
    {
        OpenedTournament = openedTournament;
    }
}
