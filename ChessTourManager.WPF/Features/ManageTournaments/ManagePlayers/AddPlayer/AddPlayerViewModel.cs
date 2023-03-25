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
        Team = team;
    }

    public AddPlayerViewModel()
    {
    }

    public Team? Team { get; }

    public string PlayerFirstName
    {
        get { return _playerFirstName ?? "Введите имя"; }
        set { SetField(ref _playerFirstName, value); }
    }

    public string PlayerLastName
    {
        get { return _playerLastName ?? "Введите фамилию"; }
        set { SetField(ref _playerLastName, value); }
    }

    public ICommand CompleteAddPlayerCommand
    {
        get { return _completeAddPlayerCommand ??= new CompleteAddPlayerCommand(this); }
    }

    public char Gender
    {
        get { return _gender ?? 'М'; }
        set { SetField(ref _gender, value); }
    }
}
