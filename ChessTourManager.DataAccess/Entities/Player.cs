using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChessTourManager.DataAccess.Entities;

public class Player : INotifyPropertyChanged
{
    private       string?    _playerLastName;
    private       string?    _playerFirstName;
    private       char       _gender = 'M';
    private       string?    _playerAttribute;
    private       int        _playerBirthYear = DateTime.Now.Year - 10;
    private       bool?      _isActive;
    private       double     _pointsAmount;
    private       int        _winsCount;
    private       int        _lossesCount;
    private       int        _drawsCount;
    private       decimal    _ratioSum1;
    private       decimal    _ratioSum2;
    private       int        _boardNumber = 1;
    private       int?       _teamId;
    private       int?       _groupId;
    private       Group?     _group;
    private       Team?      _team;
    private       Tournament _tournament;

    public int Id { get; set; }

    public int TournamentId { get; set; }

    public int OrganizerId { get; set; }

    [DisplayName("Last name")]
    [MinLength(2, ErrorMessage = "The last name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "The last name must be no more than 50 characters long.")]
    [Required]
    public string PlayerLastName
    {
        get { return this._playerLastName; }
        set { this.SetField(ref this._playerLastName, value); }
    }

    [DisplayName("First name")]
    [MinLength(2, ErrorMessage = "The first name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "The first name must be no more than 50 characters long.")]
    [Required]
    public string PlayerFirstName
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

    [DisplayName("Gender")]
    public char Gender
    {
        get { return this._gender; }
        set { this.SetField(ref this._gender, value); }
    }

    [DisplayName("Attribute")]
    [MinLength(1, ErrorMessage = "The player attribute must be at least 1 characters long.")]
    [MaxLength(4, ErrorMessage = "The player attribute must be no more than 4 characters long.")]
    public string? PlayerAttribute
    {
        get { return this._playerAttribute; }
        set { this.SetField(ref this._playerAttribute, value); }
    }

    [DisplayName("Birth year")]
    public int PlayerBirthYear
    {
        get { return this._playerBirthYear; }
        set
        {
            if (value < 1900)
            {
                return;
            }

            this.SetField(ref this._playerBirthYear, value);
        }
    }

    [DisplayName("Active")]
    public bool? IsActive
    {
        get { return this._isActive; }
        set { this.SetField(ref this._isActive, value); }
    }

    [DisplayName("Points")]
    public double PointsAmount
    {
        get { return this._pointsAmount; }
        set
        {
            if (value < 0)
            {
                return;
            }

            this.SetField(ref this._pointsAmount, value);
        }
    }

    [DisplayName("Wins")]
    public int WinsCount
    {
        get { return this._winsCount; }
        set
        {
            if (value < 0)
            {
                return;
            }

            this.SetField(ref this._winsCount, value);
        }
    }

    [DisplayName("Losses")]
    public int LossesCount
    {
        get { return this._lossesCount; }
        set
        {
            if (value < 0)
            {
                return;
            }

            this.SetField(ref this._lossesCount, value);
        }
    }

    [DisplayName("Draws")]
    public int DrawsCount
    {
        get { return this._drawsCount; }
        set
        {
            if (value < 0)
            {
                return;
            }

            this.SetField(ref this._drawsCount, value);
        }
    }

    [DisplayName("Ratio 1")]
    public decimal RatioSum1
    {
        get { return this._ratioSum1; }
        set
        {
            if (value < 0)
            {
                return;
            }

            this.SetField(ref this._ratioSum1, value);
        }
    }

    [DisplayName("Ratio 2")]
    public decimal RatioSum2
    {
        get { return this._ratioSum2; }
        set
        {
            if (value < 0)
            {
                return;
            }

            this.SetField(ref this._ratioSum2, value);
        }
    }

    [DisplayName("Board")]
    [Range(1, 15, ErrorMessage = "The board number must be between 1 and 15.")]
    public int BoardNumber
    {
        get { return this._boardNumber; }
        set
        {
            if (value < 1)
            {
                return;
            }

            this.SetField(ref this._boardNumber, value);
        }
    }

    [DisplayName("Team")]
    public int? TeamId
    {
        get { return this._teamId; }
        set { this.SetField(ref this._teamId, value); }
    }

    [DisplayName("Group")]
    public int? GroupId
    {
        get { return this._groupId; }
        set { this.SetField(ref this._groupId, value); }
    }

    public ICollection<Game> GamesBlackOpponents { get; } = new List<Game>();

    public ICollection<Game> GamesWhiteOpponents { get; } = new List<Game>();

    public string IsActiveLocalized =>
        this.IsActive == true
            ? "Yes"
            : "No";

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

    public Tournament? Tournament
    {
        get { return this._tournament; }
        set { this.SetField(ref this._tournament, value); }
    }

    [NotMapped]
    [DisplayName("Full name")]
    public string PlayerFullName
    {
        get { return this.PlayerLastName + " " + this.PlayerFirstName; }
    }

    public override string ToString()
    {
        return this.Id + " " + this.PlayerFullName;
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
