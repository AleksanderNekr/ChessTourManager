﻿using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public class AddPlayerCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (MainViewModel.SelectedTournament == null)
        {
            return;
        }

        IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext)
                      .TryAddPlayer(out Player? player, TournamentsListViewModel.SelectedTournament.TournamentId,
                                    TournamentsListViewModel.SelectedTournament.OrganizerId,
                                    "", "");
        PlayerAddedEvent.OnPlayerAdded(new PlayerAddedEventArgs(player));
    }
}
