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
        this._manageGroupsViewModel = manageGroupsViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (MainViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return;
        }

        InsertResult result = IInsertQueries.CreateInstance(PlayersViewModel.PlayersContext)
                                            .TryAddGroup(out Group? group,
                                                         LoginViewModel.CurrentUser.Id,
                                                         MainViewModel.SelectedTournament.TournamentId, this._manageGroupsViewModel.GroupName, this._manageGroupsViewModel.GroupIdentifier);

        if (result == InsertResult.Fail)
        {
            MessageBox.Show("Не удалось добавить группу! Возможно группа с такими данными уже существует",
                            "Добавление группы", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MessageBox.Show("Группа успешно добавлена!", "Добавление группы", MessageBoxButton.OK,
                        MessageBoxImage.Information);
        GroupAddedEvent.OnGroupAdded(this, new GroupAddedEventArgs(group));
    }
}
