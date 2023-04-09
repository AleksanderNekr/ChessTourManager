﻿using System;
using System.Windows;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public class CompleteEditTeamCommand : CommandBase
{
    private readonly EditTeamViewModel _editTeamViewModel;

    public CompleteEditTeamCommand(EditTeamViewModel editTeamViewModel)
    {
        _editTeamViewModel = editTeamViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (_editTeamViewModel.Team is null)
        {
            MessageBox.Show("Не удалось сохранить изменения. Команда не найдена.",
                            "Ошибка сохранения", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        MessageBoxResult result =
            MessageBox.Show("Вы действительно хотите сохранить изменения в"
                          + $" команде {_editTeamViewModel.Team.TeamName}?",
                            "Сохранение изменений", MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        UpdateTeam();
    }

    private void UpdateTeam()
    {
        try
        {
            _editTeamViewModel.Team.TeamName      = _editTeamViewModel.Name;
            _editTeamViewModel.Team.TeamAttribute = _editTeamViewModel.Attribute;
            _editTeamViewModel.Team.IsActive      = _editTeamViewModel.IsActive;

            ManageTeamsViewModel.TeamsContext.Teams.Update(_editTeamViewModel.Team);

            ManageTeamsViewModel.TeamsContext.SaveChanges();

            TeamEditedEvent.OnTeamChanged(this, new TeamChangedEventArgs(_editTeamViewModel.Team));
            MessageBox.Show("Изменения в команде успешно сохранены!", "Сохранение изменений",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show("Не удалось сохранить изменения в команде!\n" + e.Message,
                            "Ошибка сохранения", MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}
