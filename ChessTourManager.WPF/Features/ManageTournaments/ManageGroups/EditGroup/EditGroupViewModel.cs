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
        this.Group    = group;
        this.SaveCommand = new SaveGroupCommand(this);
    }

    public EditGroupViewModel()
    {
        this.Group    = null;
        this.SaveCommand = new SaveGroupCommand(this);
    }

    internal Group? Group { get; }

    public ICommand SaveCommand { get; }

    public string GroupName
    {
        get { return this._groupName ??= this.Group?.GroupName ?? string.Empty; }
        set { this.SetField(ref this._groupName, value); }
    }

    public string GroupIdentity
    {
        get { return this._groupIdentity ??= this.Group?.Identity ?? string.Empty; }
        set { this.SetField(ref this._groupIdentity, value); }
    }
}
