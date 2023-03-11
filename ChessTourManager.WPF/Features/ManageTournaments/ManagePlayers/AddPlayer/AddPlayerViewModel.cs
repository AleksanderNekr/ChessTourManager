using System.Windows.Input;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;

public class AddPlayerViewModel : ViewModelBase
{
    private string?                   _playerLastName;
    private string?                   _playerFirstName;
    private CompleteAddPlayerCommand? _completeAddPlayerCommand;
    private char?                     _gender;

    public Team Team { get; }

    public AddPlayerViewModel(Team team)
    {
        Team = team;
    }

    public AddPlayerViewModel()
    {
    }


    public string PlayerFirstName
    {
        get { return _playerFirstName ?? string.Empty; }
        set { SetField(ref _playerFirstName, value); }
    }

    public string PlayerLastName
    {
        get { return _playerLastName ?? string.Empty; }
        set { SetField(ref _playerLastName, value); }
    }

    public ICommand CompleteAddPlayerCommand => _completeAddPlayerCommand ??= new CompleteAddPlayerCommand(this);

    public char Gender
    {
        get { return _gender ?? 'М'; }
        set { SetField(ref _gender, value); }
    }
}
