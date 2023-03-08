using System.Windows;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;

public class DeleteTeamCommand : CommandBase
{
    private readonly ChessTourContext _context;

    public DeleteTeamCommand()
    {
        _context = ManageTeamsViewModel.TeamsContext;
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

        _context.Teams.Remove(team);
        _context.SaveChanges();
        TeamDeletedEvent.OnTeamDeleted(new TeamDeletedEventArgs(team));
    }
}
