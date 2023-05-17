using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;

public class EditPlayerViewModel : ViewModelBase
{
    private CompleteEditPlayerCommand? _completeEditPlayerCommand;
    private char                       _gender;
    private string?                    _playerFirstName;
    private string?                    _playerLastName;

    public EditPlayerViewModel()
    {
    }

    public EditPlayerViewModel(Player? player)
    {
        this.Player          = player;
        this.PlayerLastName  = this.Player?.PlayerLastName;
        this.PlayerFirstName = this.Player?.PlayerFirstName;
        if (this.Player is not null)
        {
            this.Gender = this.Player.Gender;
        }
    }

    internal Player? Player { get; }

    public string? PlayerFirstName
    {
        get { return this._playerFirstName; }
        set { this.SetField(ref this._playerFirstName, value); }
    }

    public string? PlayerLastName
    {
        get { return this._playerLastName; }
        set { this.SetField(ref this._playerLastName, value); }
    }

    public ICommand CompleteEditPlayerCommand
    {
        get { return this._completeEditPlayerCommand ??= new CompleteEditPlayerCommand(this); }
    }

    public char Gender
    {
        get { return this._gender; }
        set { this.SetField(ref this._gender, value); }
    }
}
