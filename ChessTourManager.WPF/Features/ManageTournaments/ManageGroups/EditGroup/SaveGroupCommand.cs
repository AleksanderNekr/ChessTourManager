using System.Windows;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public class SaveGroupCommand : CommandBase
{
    private readonly EditGroupViewModel _editGroupViewModel;

    public SaveGroupCommand(EditGroupViewModel editGroupViewModel)
    {
        this._editGroupViewModel = editGroupViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (this._editGroupViewModel is { Group: null })
        {
            MessageBox.Show("Не удалось сохранить изменения. Группа не найдена.",
                            "Ошибка сохранения", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        MessageBoxResult result =
            MessageBox.Show($"Вы действительно хотите сохранить изменения в группе {this._editGroupViewModel.Group.GroupName}?",
                            "Сохранение изменений", MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        this._editGroupViewModel.Group.GroupName = this._editGroupViewModel.GroupName;
        this._editGroupViewModel.Group.Identity  = this._editGroupViewModel.GroupIdentity;

        PlayersViewModel.PlayersContext.Groups.Update(this._editGroupViewModel.Group);
        PlayersViewModel.PlayersContext.SaveChanges();

        GroupChangedEvent.OnGroupChanged(this, new GroupChangedEventArgs(this._editGroupViewModel.Group));

        MessageBox.Show("Изменения в группе успешно сохранены!", "Сохранение изменений",
                        MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
