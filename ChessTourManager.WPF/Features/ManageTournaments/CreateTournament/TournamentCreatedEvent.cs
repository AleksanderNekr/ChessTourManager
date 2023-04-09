using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public static class TournamentCreatedEvent
{
    public delegate void TournamentCreatedHandler(object source, TournamentCreatedEventArgs e);

    public static event TournamentCreatedHandler? TournamentCreated;

    internal static void OnTournamentCreated(object source, TournamentCreatedEventArgs e)
    {
        TournamentCreated?.Invoke(source, e);
    }
}

public class TournamentCreatedEventArgs : EventArgs
{
    public TournamentCreatedEventArgs(Tournament tournament)
    {
        Tournament = tournament;
    }

    public Tournament Tournament { get; }
}
