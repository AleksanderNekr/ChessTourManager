using System;
using System.Windows;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public class CompleteEditTeamCommand : CommandBase
{
    private readonly EditTeamViewModel _editTeamViewModel;

    public CompleteEditTeamCommand(EditTeamViewModel editTeamViewModel)
    {
        this._editTeamViewModel = editTeamViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (this._editTeamViewModel.Team is null)
        {
            MessageBox.Show("Не удалось сохранить изменения. Команда не найдена.",
                            "Ошибка сохранения", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        MessageBoxResult result =
            MessageBox.Show("Вы действительно хотите сохранить изменения в"
                          + $" команде {this._editTeamViewModel.Team.TeamName}?",
                            "Сохранение изменений", MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        this.UpdateTeam();
    }

    private void UpdateTeam()
    {
        try
        {
            this._editTeamViewModel.Team.TeamName      = this._editTeamViewModel.Name;
            this._editTeamViewModel.Team.TeamAttribute = this._editTeamViewModel.Attribute;
            this._editTeamViewModel.Team.IsActive      = this._editTeamViewModel.IsActive;

            PlayersViewModel.PlayersContext.Teams.Update(this._editTeamViewModel.Team);

            PlayersViewModel.PlayersContext.SaveChanges();

            TeamChangedEvent.OnTeamChanged(this, new TeamChangedEventArgs(this._editTeamViewModel.Team));
            MessageBox.Show("Изменения в команде успешно сохранены!", "Сохранение изменений",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show("Не удалось сохранить изменения в команде!\n"
                          + "Возможнно команда с таким именем уже существует",
                            "Ошибка сохранения", MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}
