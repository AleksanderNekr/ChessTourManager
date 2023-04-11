using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.Domain.Algorithms;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;

public class StartNewTourCommand : CommandBase
{
    private static IDrawingAlgorithm _drawingAlgorithm;

    private readonly PairsGridViewModel _pairsGridViewModel;

    public StartNewTourCommand(PairsGridViewModel pairsGridViewModel)
    {
        _pairsGridViewModel = pairsGridViewModel;
        _drawingAlgorithm = IDrawingAlgorithm.Initialize(PlayersViewModel.PlayersContext,
                                                         pairsGridViewModel.OpenedTournament);
    }

    public override void Execute(object? parameter)
    {
        if (_pairsGridViewModel.OpenedTournament is null)
        {
            MessageBox.Show("Турнир не открыт", "Жеребьевка турнира",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (_pairsGridViewModel.CurrentTour == _pairsGridViewModel.OpenedTournament.ToursCount)
        {
            MessageBox.Show("Турнир завершен", "Жеребьевка турнира",
                            MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        List<(int, int)> idPairs = new(_drawingAlgorithm.StartNewTour(_pairsGridViewModel.CurrentTour));
        if (idPairs.Count == 0)
        {
            MessageBox.Show("Недостаточно игроков для проведения тура", "Жеребьевка турнира",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        foreach ((int, int) idPair in idPairs.Where(idPair => idPair.Item1 != -1 && idPair.Item2 != -1))
        {
            InsertResult result = IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext)
                                                .TryAddGamePair(out Game? game,
                                                                idPair.Item1, idPair.Item2,
                                                                _pairsGridViewModel.OpenedTournament.TournamentId,
                                                                _pairsGridViewModel.OpenedTournament.OrganizerId,
                                                                _drawingAlgorithm.NewTourNumber);

            if (result == InsertResult.Fail)
            {
                MessageBox.Show("Ошибка в веденных данных! Возможно пара с такими данными уже существует,"
                              + " либо вы не заполнили важные данные", "Ошибка при добавлении пары",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GameAddedEvent.OnGameAdded(this, new GameAddedEventArgs(game));
        }

        TourAddedEvent.OnTourAdded(this, new TourAddedEventArgs(_drawingAlgorithm.NewTourNumber));
    }
}
