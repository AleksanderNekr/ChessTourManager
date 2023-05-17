using System;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public class CreateTournamentCommand : CommandBase
{
    private readonly CreateTournamentViewModel _createViewModel;

    public CreateTournamentCommand(CreateTournamentViewModel createViewModel)
    {
        this._createViewModel = createViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (LoginViewModel.CurrentUser == null
         || this._createViewModel is not { SelectedTournamentSystem: not null, SelectedTournamentKind: not null })
        {
            return;
        }

        string? name = this._createViewModel.TournamentNameText?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("Введите название турнира!", "Создание турнира", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        InsertResult result = IInsertQueries
                             .CreateInstance(CreateTournamentViewModel.CreateTournamentContext)
                             .TryAddTournament(
                                               out Tournament? tournament,
                                               LoginViewModel.CurrentUser.Id,
                                               name, this._createViewModel.SelectedTournamentSystem
                                                                           .Id, this._createViewModel
                                                  .SelectedTournamentKind
                                                  .Id, this._createViewModel
                                                           .SelectedTournamentRoundsCount,
                                               this._createViewModel.TournamentPlaceText?.Trim(),
                                               DateOnly.FromDateTime((DateTime)this._createViewModel.SelectedDate),
                                               this._createViewModel.SelectedTime,
                                               this._createViewModel.SelectedDurationHours,
                                               this._createViewModel.SelectedMaxTeamPlayers,
                                               this._createViewModel.OrgNameText?.Trim(),
                                               this._createViewModel.IsMixedGroupsAllowed
                                              );
        if (result == InsertResult.Fail)
        {
            MessageBox.Show("Не удалось создать турнир! Возможно турнир с такими данными уже существует",
                            "Создание турнира", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MessageBox.Show("Турнир успешно создан!", "Создание турнира", MessageBoxButton.OK,
                        MessageBoxImage.Information);
        TournamentCreatedEvent.OnTournamentCreated(this, new TournamentCreatedEventArgs(tournament));
    }
}
