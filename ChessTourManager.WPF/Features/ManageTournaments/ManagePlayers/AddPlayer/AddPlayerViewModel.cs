using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public class AddPlayerViewModel : ViewModelBase
{
    private CompleteAddPlayerCommand? _completeAddPlayerCommand;
    private char?                     _gender;
    private string?                   _playerFirstName;
    private string?                   _playerLastName;

    public AddPlayerViewModel(Team? team)
    {
        this.Team = team;
    }

    public AddPlayerViewModel()
    {
    }

    public Team? Team { get; }

    public string PlayerFirstName
    {
        get { return this._playerFirstName ?? string.Empty; }
        set { this.SetField(ref this._playerFirstName, value); }
    }

    public string PlayerLastName
    {
        get { return this._playerLastName ?? string.Empty; }
        set { this.SetField(ref this._playerLastName, value); }
    }

    public ICommand CompleteAddPlayerCommand
    {
        get { return this._completeAddPlayerCommand ??= new CompleteAddPlayerCommand(this); }
    }

    public char Gender
    {
        get { return this._gender ?? 'М'; }
        set { this.SetField(ref this._gender, value); }
    }
}
