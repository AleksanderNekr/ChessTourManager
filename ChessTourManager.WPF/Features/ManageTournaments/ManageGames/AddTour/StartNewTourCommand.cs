using System.Collections.Generic;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.Domain.Algorithms;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;

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
            InsertResult result = IInsertQueries.CreateInstance(PairsGridViewModel.PairsContext)
                                                .TryAddGamePair(out Game? game, idPair.Item1, idPair.Item2,
                                                                TournamentsListViewModel.SelectedTournament!
                                                                   .TournamentId,
                                                                TournamentsListViewModel.SelectedTournament
                                                                   .OrganizerId,
                                                                RoundRobin.NewTourNumber);

            if (result == InsertResult.Fail)
            {
                MessageBox.Show("Ошибка в веденных данных! Возможно пара с такими данными уже существует,"
                              + " либо вы не заполнили важные данные", "Ошибка при добавлении пары",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GameAddedEvent.OnGameAdded(this, new GameAddedEventArgs(game!));
        }

        TourAddedEvent.OnTourAdded(this, new TourAddedEventArgs(RoundRobin.NewTourNumber));
    }
}
