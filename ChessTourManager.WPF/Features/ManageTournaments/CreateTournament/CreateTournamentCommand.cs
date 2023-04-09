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
        _createViewModel = createViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (LoginViewModel.CurrentUser == null
         || _createViewModel is not { SelectedTournamentSystem: { }, SelectedTournamentKind: { } })
        {
            return;
        }

        InsertResult result = IInsertQueries
                             .CreateInstance(CreateTournamentViewModel.CreateTournamentContext)
                             .TryAddTournament(
                                               out Tournament? tournament,
                                               LoginViewModel.CurrentUser.UserId,
                                               _createViewModel.TournamentNameText?.Trim(),
                                               _createViewModel.SelectedTournamentSystem
                                                               .SystemId,
                                               _createViewModel.SelectedTournamentKind
                                                               .KindId,
                                               _createViewModel
                                                  .SelectedTournamentRoundsCount,
                                               _createViewModel.TournamentPlaceText?.Trim(),
                                               DateOnly.FromDateTime((DateTime)_createViewModel.SelectedDate),
                                               _createViewModel.SelectedTime,
                                               _createViewModel.SelectedDurationHours,
                                               _createViewModel.SelectedMaxTeamPlayers,
                                               _createViewModel.OrgNameText?.Trim(),
                                               _createViewModel.IsMixedGroupsAllowed
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
