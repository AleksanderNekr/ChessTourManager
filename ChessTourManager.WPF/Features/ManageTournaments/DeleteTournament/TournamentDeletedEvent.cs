using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.DeleteTournament;

public static class TournamentDeletedEvent
{
    public delegate void DeleteTournamentHandler(DeleteTournamentEventArgs e);

    public static event DeleteTournamentHandler? TournamentDeleted;

    internal static void OnTournamentDeleted(DeleteTournamentEventArgs e)
    {
        TournamentDeleted?.Invoke(e);
    }
}

public class DeleteTournamentEventArgs : EventArgs
{
    public DeleteTournamentEventArgs(Tournament deletedTournament)
    {
        DeletedTournament = deletedTournament;
    }

    public Tournament DeletedTournament { get; }
}
