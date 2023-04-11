using System;
using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;

public class DeleteTeamCommand : CommandBase
{
    private readonly ChessTourContext _context;

    public DeleteTeamCommand()
    {
        _context = PlayersViewModel.PlayersContext;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is not Team team)
        {
            return;
        }

        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить команду {team.TeamName}?",
                                                  "Удаление команды", MessageBoxButton.YesNo,
                                                  MessageBoxImage.Question);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        try
        {
            _context.Teams.Remove(team);
            _context.SaveChanges();
            TeamDeletedEvent.OnTeamDeleted(this, new TeamDeletedEventArgs(team));
            MessageBox.Show($"Команда {team.TeamName} успешно удалена!", "Удаление команды",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show("Не удалось удалить команду!\n" + e.Message, "Удаление команды",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
