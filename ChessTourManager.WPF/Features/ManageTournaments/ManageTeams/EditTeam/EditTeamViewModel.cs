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
        Team        = team;
        SaveCommand = new CompleteEditTeamCommand(this);
    }

    public EditTeamViewModel()
    {
        Team        = null;
        SaveCommand = new CompleteEditTeamCommand(this);
    }

    internal Team? Team { get; }

    public string Name
    {
        get { return _name ??= Team?.TeamName ?? string.Empty; }
        set { SetField(ref _name, value); }
    }

    public string Attribute
    {
        get { return _attribute ??= Team?.TeamAttribute ?? string.Empty; }
        set { SetField(ref _attribute, value); }
    }

    public bool IsActive
    {
        get { return _isActive ??= Team?.IsActive ?? true; }
        set { SetField(ref _isActive, value); }
    }

    public ICommand SaveCommand { get; }
}
