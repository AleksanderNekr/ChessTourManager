using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public class EditGroupViewModel : ViewModelBase
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
}
