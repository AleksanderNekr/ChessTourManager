using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChessTourManager.DataAccess.Entities;

public class Player : INotifyPropertyChanged
{
    private string?    _playerLastName;
    private string?    _playerFirstName;
    private char       _gender;
    private string?    _playerAttribute;
    private int        _playerBirthYear = DateTime.Now.Year - 10;
    private bool?      _isActive;
    private double     _pointsAmount;
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
    private Tournament _tournament;

    public int PlayerId { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    public string? PlayerLastName
    {
        get { return this._playerLastName; }
        set { this.SetField(ref this._playerLastName, value); }
    }

    public string? PlayerFirstName
    {
        get { return this._playerFirstName; }
        set
        {
            if (this.SetField(ref this._playerFirstName, value))
            {
                this.OnPropertyChanged(nameof(this.PlayerFullName));
            }
        }
    }

    public char Gender
    {
        get { return this._gender; }
        set { this.SetField(ref this._gender, value); }
    }

    public string PlayerAttribute
    {
        get { return this._playerAttribute; }
        set { this.SetField(ref this._playerAttribute, value); }
    }

    public int PlayerBirthYear
    {
        get { return this._playerBirthYear; }
        set { this.SetField(ref this._playerBirthYear, value); }
    }

    public bool? IsActive
    {
        get { return this._isActive; }
        set { this.SetField(ref this._isActive, value); }
    }

    public double PointsAmount
    {
        get { return this._pointsAmount; }
        set { this.SetField(ref this._pointsAmount, value); }
    }

    public int WinsCount
    {
        get { return this._winsCount; }
        set { this.SetField(ref this._winsCount, value); }
    }

    public int LossesCount
    {
        get { return this._lossesCount; }
        set { this.SetField(ref this._lossesCount, value); }
    }

    public int DrawsCount
    {
        get { return this._drawsCount; }
        set { this.SetField(ref this._drawsCount, value); }
    }

    public decimal RatioSum1
    {
        get { return this._ratioSum1; }
        set { this.SetField(ref this._ratioSum1, value); }
    }

    public decimal RatioSum2
    {
        get { return this._ratioSum2; }
        set { this.SetField(ref this._ratioSum2, value); }
    }

    public int BoardNumber
    {
        get { return this._boardNumber; }
        set { this.SetField(ref this._boardNumber, value); }
    }

    public int? TeamId
    {
        get { return this._teamId; }
        set { this.SetField(ref this._teamId, value); }
    }

    public int? GroupId
    {
        get { return this._groupId; }
        set { this.SetField(ref this._groupId, value); }
    }

    public ICollection<Game> BlackGamePlayers { get; } = new List<Game>();

    public ICollection<Game> WhiteGamePlayers { get; } = new List<Game>();

    public Group? Group
    {
        get { return this._group; }
        set { this.SetField(ref this._group, value); }
    }

    public Team? Team
    {
        get { return this._team; }
        set { this.SetField(ref this._team, value); }
    }

    public Tournament Tournament
    {
        get { return this._tournament; }
        set { this.SetField(ref this._tournament, value); }
    }

    [NotMapped]
    public string PlayerFullName
    {
        get { return this.PlayerLastName + " " + this.PlayerFirstName; }
    }

    public override string ToString()
    {
        return this.PlayerId + " " + this.PlayerFullName;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        this.OnPropertyChanged(propertyName);
        return true;
    }
}
