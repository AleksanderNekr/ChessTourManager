using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public class EditTeamViewModel : ViewModelBase
{
    private string? _attribute;
    private bool?   _isActive;
    private string? _name;

    public EditTeamViewModel(Team? team)
    {
        this.Team     = team;
        this.SaveCommand = new CompleteEditTeamCommand(this);
    }

    public EditTeamViewModel()
    {
        this.Team     = null;
        this.SaveCommand = new CompleteEditTeamCommand(this);
    }

    internal Team? Team { get; }

    public string Name
    {
        get { return this._name ??= this.Team?.TeamName ?? string.Empty; }
        set { this.SetField(ref this._name, value); }
    }

    public string Attribute
    {
        get { return this._attribute ??= this.Team?.TeamAttribute ?? string.Empty; }
        set { this.SetField(ref this._attribute, value); }
    }

    public bool IsActive
    {
        get { return this._isActive ??= this.Team?.IsActive ?? true; }
        set { this.SetField(ref this._isActive, value); }
    }

    public ICommand SaveCommand { get; }
}
