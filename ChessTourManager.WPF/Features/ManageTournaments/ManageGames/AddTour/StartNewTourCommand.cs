using System.Collections.Generic;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.Domain.Algorithms;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;

public class StartNewTourCommand : CommandBase
{
    private static readonly IDrawingAlgorithm DrawingAlgorithm = IDrawingAlgorithm.Initialize(PairsGridViewModel.PairsContext,
                                                                            TournamentsListViewModel
                                                                               .SelectedTournament);

    private readonly PairsGridViewModel _pairsGridViewModel;

    public StartNewTourCommand(PairsGridViewModel pairsGridViewModel)
    {
        _pairsGridViewModel = pairsGridViewModel;
    }

    public override void Execute(object? parameter)
    {
        // TODO: fix it
        // Check if there are any dummy players and make them inactive if there are odd number of players.
        if (TournamentsListViewModel.SelectedTournament!.Players.Contains(_pairsGridViewModel.DummyPlayer)
         && (TournamentsListViewModel.SelectedTournament.Players.Count % 2) == 1)
        {
            _pairsGridViewModel.DummyPlayer.IsActive = false;
            PairsGridViewModel.PairsContext.SaveChanges();
        }

        List<(int, int)> idPairs = new(DrawingAlgorithm.StartNewTour(_pairsGridViewModel.CurrentTour));

        foreach ((int, int) idPair in idPairs)
        {
            Game? game;

            // If id = -1 then it means that the player is absent.
            // Then add a game with the result of 0:1 and mark as not played.
            // Also add new dummy player to the database.
            if (idPair.Item1 == -1)
            {
                IInsertQueries.CreateInstance(PairsGridViewModel.PairsContext)
                              .TryAddPlayer(out Player? dummyPlayer,
                                            TournamentsListViewModel.SelectedTournament.TournamentId,
                                            TournamentsListViewModel.SelectedTournament.OrganizerId,
                                            "<Пусто>", "");
                if (dummyPlayer is null)
                {
                    continue;
                }

                IInsertQueries.CreateInstance(PairsGridViewModel.PairsContext)
                              .TryAddGamePair(out game, dummyPlayer.PlayerId, idPair.Item2,
                                              TournamentsListViewModel.SelectedTournament!
                                                                      .TournamentId,
                                              TournamentsListViewModel.SelectedTournament.OrganizerId,
                                              DrawingAlgorithm.NewTourNumber);
                if (game != null)
                {
                    game.WhitePoints                = 0;
                    game.BlackPoints                = 1;
                    game.IsPlayed                   = false;
                    _pairsGridViewModel.DummyPlayer = dummyPlayer;

                    GameAddedEvent.OnGameAdded(this, new GameAddedEventArgs(game));
                }

                continue;
            }

            if (idPair.Item2 == -1)
            {
                IInsertQueries.CreateInstance(PairsGridViewModel.PairsContext)
                              .TryAddPlayer(out Player? dummyPlayer,
                                            TournamentsListViewModel.SelectedTournament.TournamentId,
                                            TournamentsListViewModel.SelectedTournament.OrganizerId,
                                            "<Пусто>", "<Пусто>");
                if (dummyPlayer is null)
                {
                    continue;
                }

                IInsertQueries.CreateInstance(PairsGridViewModel.PairsContext)
                              .TryAddGamePair(out game, idPair.Item1, dummyPlayer.PlayerId,
                                              TournamentsListViewModel.SelectedTournament!
                                                                      .TournamentId,
                                              TournamentsListViewModel.SelectedTournament.OrganizerId,
                                              DrawingAlgorithm.NewTourNumber);
                if (game != null)
                {
                    game.WhitePoints                = 1;
                    game.BlackPoints                = 0;
                    game.IsPlayed                   = false;
                    _pairsGridViewModel.DummyPlayer = dummyPlayer;

                    GameAddedEvent.OnGameAdded(this, new GameAddedEventArgs(game));
                }

                continue;
            }

            InsertResult result = IInsertQueries.CreateInstance(PairsGridViewModel.PairsContext)
                                                .TryAddGamePair(out game, idPair.Item1, idPair.Item2,
                                                                TournamentsListViewModel.SelectedTournament!
                                                                   .TournamentId,
                                                                TournamentsListViewModel.SelectedTournament
                                                                   .OrganizerId,
                                                                DrawingAlgorithm.NewTourNumber);

            if (result == InsertResult.Fail)
            {
                MessageBox.Show("Ошибка в веденных данных! Возможно пара с такими данными уже существует,"
                              + " либо вы не заполнили важные данные", "Ошибка при добавлении пары",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GameAddedEvent.OnGameAdded(this, new GameAddedEventArgs(game!));
        }

        TourAddedEvent.OnTourAdded(this, new TourAddedEventArgs(DrawingAlgorithm.NewTourNumber));
    }
}
