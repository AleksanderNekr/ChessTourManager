using System.Windows;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;

public class CompleteAddGroupCommand : CommandBase
{
    private readonly ManageGroupsViewModel _manageGroupsViewModel;

    public CompleteAddGroupCommand(ManageGroupsViewModel manageGroupsViewModel)
    {
        _manageGroupsViewModel = manageGroupsViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (TournamentsListViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return;
        }

        IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext).TryAddGroup(out Group? group,
            LoginViewModel.CurrentUser.UserId, TournamentsListViewModel.SelectedTournament.TournamentId,
            _manageGroupsViewModel.GroupName, _manageGroupsViewModel.GroupIdentifier);

        if (group is { })
        {
            MessageBox.Show("Группа успешно добавлена!", "Добавление группы", MessageBoxButton.OK,
                            MessageBoxImage.Information);
            GroupAddedEvent.OnGroupAdded(new GroupAddedEventArgs(group));
        }
    }
}
