using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public class EditGroupViewModel : ViewModelBase, IValidatableObject
{
    private string? _groupIdentity;
    private string? _groupName;

    public EditGroupViewModel(Group? group)
    {
        Group       = group;
        SaveCommand = new SaveGroupCommand(this);
    }

    public EditGroupViewModel()
    {
        Group       = null;
        SaveCommand = new SaveGroupCommand(this);
    }

    internal Group? Group { get; }

    public ICommand SaveCommand { get; }

    public string GroupName
    {
        get { return _groupName ??= Group?.GroupName ?? string.Empty; }
        set { SetField(ref _groupName, value); }
    }

    public string GroupIdentity
    {
        get { return _groupIdentity ??= Group?.Identity ?? string.Empty; }
        set { SetField(ref _groupIdentity, value); }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(GroupName))
        {
            yield return new ValidationResult("Название группы обязательно!", new[] { nameof(GroupName) });
        }

        if (string.IsNullOrWhiteSpace(GroupName))
        {
            yield return new ValidationResult("Название группы обязательно!", new[] { nameof(GroupName) });
        }

        // Check if the group name is unique
        if (ManageGroupsViewModel.GroupsContext.Groups.Any(t => Group != null && t.GroupName == GroupName
                                                             && t.GroupId                    != Group.GroupId))
        {
            yield return new ValidationResult("Группа с таким названием уже существует!",
                                              new[] { nameof(GroupName) });
        }
    }
}
