using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

public class EditTeamViewModel : ViewModelBase, IValidatableObject
{
    private string? _attribute;
    private bool?   _isActive;
    private string? _name;

    public EditTeamViewModel(Team? team)
    {
        Team        = team;
        SaveCommand = new SaveTeamCommand(this);
    }

    public EditTeamViewModel()
    {
        Team        = null;
        SaveCommand = new SaveTeamCommand(this);
    }

    internal Team? Team { get; }

    public string Name
    {
        get { return _name ??= Team?.TeamName ?? string.Empty; }
        set { SetField(ref _name, value); }
    }

    public string Attribute
    {
        get { return _attribute ??= Team?.TeamAttribute ?? "---"; }
        set { SetField(ref _attribute, value); }
    }

    public bool IsActive
    {
        get { return _isActive ??= Team?.IsActive ?? true; }
        set { SetField(ref _isActive, value); }
    }

    public ICommand SaveCommand { get; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult("Имя команды обязательно!", new[] { nameof(Name) });
        }

        // Check if the team name is unique
        if (ManageTeamsViewModel.TeamsContext.Teams.Any(t => Team != null && t.TeamName == Name
                                                                          && t.TeamId   != Team.TeamId))
        {
            yield return new ValidationResult("Команда с таким именем уже существует!", new[] { nameof(Name) });
        }
    }
}
