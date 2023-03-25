using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;

public class EditPlayerViewModel : ViewModelBase
{
    private CompleteEditPlayerCommand? _completeEditPlayerCommand;
    private char                       _gender;
    private string                     _playerFirstName;
    private string                     _playerLastName;

    public EditPlayerViewModel()
    {
    }

    public EditPlayerViewModel(Player? player)
    {
        Player          = player;
        PlayerLastName  = Player.PlayerLastName;
        PlayerFirstName = Player.PlayerFirstName;
        Gender          = Player.Gender;
    }

    internal Player? Player { get; set; }

    public string PlayerFirstName
    {
        get { return _playerFirstName; }
        set { SetField(ref _playerFirstName, value); }
    }

    public string PlayerLastName
    {
        get { return _playerLastName; }
        set { SetField(ref _playerLastName, value); }
    }

    public ICommand CompleteEditPlayerCommand
    {
        get { return _completeEditPlayerCommand ??= new CompleteEditPlayerCommand(this); }
    }

    public char Gender
    {
        get { return _gender; }
        set { SetField(ref _gender, value); }
    }
}
