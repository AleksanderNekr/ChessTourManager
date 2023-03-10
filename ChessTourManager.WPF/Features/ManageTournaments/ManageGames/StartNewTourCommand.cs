using System.Collections.Generic;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.Domain.Models;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames;

public class StartNewTourCommand : CommandBase
{
    private readonly PairsGridViewModel _pairsGridViewModel;

    private static readonly IRoundRobin RoundRobin = IRoundRobin.Initialize(PairsGridViewModel.PairsContext,
                                                                            TournamentsListViewModel
                                                                               .SelectedTournament!);

    public StartNewTourCommand(PairsGridViewModel pairsGridViewModel)
    {
        _pairsGridViewModel = pairsGridViewModel;
    }

    public override void Execute(object? parameter)
    {
        List<(int, int)> idPairs = new(RoundRobin.StartNewTour(_pairsGridViewModel.CurrentTour));

        foreach ((int, int) idPair in idPairs)
        {
            IInsertQueries.CreateInstance(PairsGridViewModel.PairsContext)
                          .TryAddGamePair(out Game? game, idPair.Item1, idPair.Item2,
                                          TournamentsListViewModel.SelectedTournament!.TournamentId,
                                          TournamentsListViewModel.SelectedTournament.OrganizerId,
                                          RoundRobin.NewTourNumber);
            if (game != null)
            {
                GameAddedEvent.OnGameAdded(this, new GameAddedEventArgs(game));
            }
        }
    }
}
