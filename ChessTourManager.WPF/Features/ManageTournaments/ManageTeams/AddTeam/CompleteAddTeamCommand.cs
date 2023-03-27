using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;

public class CompleteAddTeamCommand : CommandBase
{
    private readonly ManageTeamsViewModel _manageTeamsViewModel;

    public CompleteAddTeamCommand(ManageTeamsViewModel manageTeamsViewModel)
    {
        _manageTeamsViewModel = manageTeamsViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (TournamentsListViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return;
        }

        IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext).TryAddTeam(out Team? team,
            LoginViewModel.CurrentUser.UserId,
            TournamentsListViewModel.SelectedTournament.TournamentId,
            _manageTeamsViewModel.TeamName);

        if (team is { })
        {
            MessageBox.Show("Команда успешно добавлена!", "Добавление команды", MessageBoxButton.OK,
                            MessageBoxImage.Information);
            TeamAddedEvent.OnTeamAdded(new TeamAddedEventArgs(team));
        }
    }
}
