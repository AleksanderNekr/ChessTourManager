using System;
using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Insert;
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
        if (LoginViewModel.CurrentUser == null)
        {
            return;
        }

        InsertResult result = IInsertQueries
                             .CreateInstance(CreateTournamentViewModel.CreateTournamentContext)
                             .TryAddTournament(
                                               out Tournament? tournament,
                                               LoginViewModel.CurrentUser.UserId,
                                               _createViewModel.TournamentNameText,
                                               _createViewModel.SelectedTournamentSystem
                                                               .SystemId,
                                               _createViewModel.SelectedTournamentKind
                                                               .KindId,
                                               _createViewModel
                                                  .SelectedTournamentRoundsCount,
                                               _createViewModel.TournamentPlaceText,
                                               DateOnly.FromDateTime(_createViewModel.SelectedDate),
                                               _createViewModel.SelectedTime,
                                               _createViewModel.SelectedDurationHours,
                                               _createViewModel.SelectedMaxTeamPlayers,
                                               _createViewModel.OrgNameText,
                                               _createViewModel.IsMixedGroupsAllowed
                                              );
        if (result == InsertResult.Success)
        {
            MessageBox.Show("Турнир успешно создан!", "Создание турнира", MessageBoxButton.OK,
                            MessageBoxImage.Information);
            TournamentCreatedEvent.OnTournamentCreated(new TournamentCreatedEventArgs(tournament!));
        }
    }
}
