using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;

public static class TournamentOpenedEvent
{
    public delegate void TournamentOpenedEventHandler(object source, TournamentOpenedEventArgs e);

    public static event TournamentOpenedEventHandler? TournamentOpened;

    internal static void OnTournamentOpened(object source, TournamentOpenedEventArgs e)
    {
        TournamentOpened?.Invoke(source, e);
    }
}

public class TournamentOpenedEventArgs : EventArgs
{
    public readonly Tournament OpenedTournament;

    public TournamentOpenedEventArgs(Tournament openedTournament)
    {
        OpenedTournament = openedTournament;
    }
}
