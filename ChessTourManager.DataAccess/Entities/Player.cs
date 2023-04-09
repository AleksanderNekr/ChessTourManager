using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChessTourManager.DataAccess.Entities;

public class Player : INotifyPropertyChanged
{
    private string?    _playerLastName  = null!;
    private string?    _playerFirstName = null!;
    private char       _gender;
    private string     _playerAttribute = null!;
    private int        _playerBirthYear = 2000;
    private bool?      _isActive;
    private double     _pointsCount;
    private int        _winsCount;
    private int        _lossesCount;
    private int        _drawsCount;
    private decimal    _ratioSum1;
    private decimal    _ratioSum2;
    private int        _boardNumber;
    private int?       _teamId;
    private int?       _groupId;
    private Group?     _group;
    private Team?      _team;
    private Tournament _tournament = null!;
    public  int        PlayerId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public string? PlayerLastName
    {
        get { return _playerLastName; }
        set { SetField(ref _playerLastName, value); }
    }

    public string? PlayerFirstName
    {
        get { return _playerFirstName; }
        set
        {
            if (SetField(ref _playerFirstName, value))
            {
                OnPropertyChanged(nameof(PlayerFullName));
            }
        }
    }

    public char Gender
    {
        get { return _gender; }
        set { SetField(ref _gender, value); }
    }

    public string PlayerAttribute
    {
        get { return _playerAttribute; }
        set { SetField(ref _playerAttribute, value); }
    }

    public int PlayerBirthYear
    {
        get { return _playerBirthYear; }
        set { SetField(ref _playerBirthYear, value); }
    }

    public bool? IsActive
    {
        get { return _isActive; }
        set { SetField(ref _isActive, value); }
    }

    public double PointsCount
    {
        get { return _pointsCount; }
        set { SetField(ref _pointsCount, value); }
    }

    public int WinsCount
    {
        get { return _winsCount; }
        set { SetField(ref _winsCount, value); }
    }

    public int LossesCount
    {
        get { return _lossesCount; }
        set { SetField(ref _lossesCount, value); }
    }

    public int DrawsCount
    {
        get { return _drawsCount; }
        set { SetField(ref _drawsCount, value); }
    }

    public decimal RatioSum1
    {
        get { return _ratioSum1; }
        set { SetField(ref _ratioSum1, value); }
    }

    public decimal RatioSum2
    {
        get { return _ratioSum2; }
        set { SetField(ref _ratioSum2, value); }
    }

    public int BoardNumber
    {
        get { return _boardNumber; }
        set { SetField(ref _boardNumber, value); }
    }

    public int? TeamId
    {
        get { return _teamId; }
        set { SetField(ref _teamId, value); }
    }

    public int? GroupId
    {
        get { return _groupId; }
        set { SetField(ref _groupId, value); }
    }

    public virtual ICollection<Game> BlackGamePlayers { get; } = new List<Game>();

    public virtual ICollection<Game> WhiteGamePlayers { get; } = new List<Game>();

    public virtual Group? Group
    {
        get { return _group; }
        set { SetField(ref _group, value); }
    }

    public virtual Team? Team
    {
        get { return _team; }
        set { SetField(ref _team, value); }
    }

    public virtual Tournament Tournament
    {
        get { return _tournament; }
        set { SetField(ref _tournament, value); }
    }

    [NotMapped]
    public string PlayerFullName
    {
        get { return PlayerLastName + " " + PlayerFirstName; }
    }

    public override string ToString()
    {
        return PlayerId + " " + PlayerFullName;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
