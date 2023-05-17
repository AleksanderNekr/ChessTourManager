using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public class CreateTournamentViewModel : ViewModelBase
{
    internal static readonly ChessTourContext CreateTournamentContext = new();

    private CreateTournamentCommand?    _createTournamentCommand;
    private string?                     _orgNameText;
    private DateTime?                   _selectedDate;
    private int?                        _selectedDurationHours;
    private int                         _selectedMaxTeamPlayers = 4;
    private TimeOnly?                   _selectedTime;
    private Kind?                       _selectedTournamentKind;
    private int?                        _selectedTournamentRoundsCount;
    private DataAccess.Entities.System? _selectedTournamentSystem;

    private ObservableCollection<Kind>? _tournamentKinds;

    private string?                                           _tournamentNameText;
    private string?                                           _tournamentPlaceText;
    private ObservableCollection<DataAccess.Entities.System>? _tournamentSystems;
    private Visibility?                                       _visibleIfTeamsAllowed;

    public ObservableCollection<Kind> TournamentKinds
    {
        get
        {
            if (this._tournamentKinds is not null)
            {
                return this._tournamentKinds;
            }

            IGetQueries.CreateInstance(CreateTournamentContext).GetKinds(out List<Kind>? kinds);
            if (kinds is not null)
            {
                this._tournamentKinds = new ObservableCollection<Kind>(kinds);
            }

            return this._tournamentKinds ??= new ObservableCollection<Kind>();
        }
    }

    public ObservableCollection<DataAccess.Entities.System> TournamentSystems
    {
        get
        {
            if (this._tournamentSystems is not null)
            {
                return this._tournamentSystems;
            }

            IGetQueries.CreateInstance(CreateTournamentContext)
                       .GetSystems(out List<DataAccess.Entities.System>? systems);
            if (systems is not null)
            {
                this._tournamentSystems = new ObservableCollection<DataAccess.Entities.System>(systems);
            }

            return this._tournamentSystems ??= new ObservableCollection<DataAccess.Entities.System>();
        }
    }

    public ObservableCollection<int> TournamentRoundsCountItems { get; } = new()
                                                                           {
                                                                               5, 6, 7, 8, 9, 10, 11, 12, 13
                                                                           };

    public string? TournamentNameText
    {
        get
        {
            if (this._tournamentNameText is null)
            {
                this.SetField(ref this._tournamentNameText, string.Empty);
            }

            return this._tournamentNameText;
        }
        set { this.SetField(ref this._tournamentNameText, value); }
    }


    public string? TournamentPlaceText
    {
        get
        {
            if (this._tournamentPlaceText is null)
            {
                this.SetField(ref this._tournamentPlaceText, string.Empty);
            }

            return this._tournamentPlaceText;
        }
        set { this.SetField(ref this._tournamentPlaceText, value); }
    }

    public DateTime? SelectedDate
    {
        get
        {
            if (this._selectedDate is null)
            {
                this.SetField(ref this._selectedDate, DateTime.Now);
            }

            return (DateTime)this._selectedDate;
        }
        set { this.SetField(ref this._selectedDate, value); }
    }

    public ObservableCollection<TimeOnly> TimeItems
    {
        get
        {
            IEnumerable<TimeOnly> times = Enumerable.Range(6, 18)
                                                    .Select(i => TimeOnly.FromDateTime(DateTime.Parse($"{i}:00")));
            return new ObservableCollection<TimeOnly>(times);
        }
    }

    public ObservableCollection<int> DurationHoursItems
    {
        get
        {
            return new ObservableCollection<int>
                   {
                       1, 2, 3, 4, 5, 6, 7, 8, 9, 10
                   };
        }
    }

    public string? OrgNameText
    {
        get
        {
            if (this._orgNameText is null)
            {
                this.SetField(ref this._orgNameText, string.Empty);
            }

            return this._orgNameText;
        }
        set { this.SetField(ref this._orgNameText, value); }
    }

    public bool IsMixedGroupsAllowed { get; set; } = true;

    public ICommand CreateTournamentCommand
    {
        get { return this._createTournamentCommand ??= new CreateTournamentCommand(this); }
    }

    public Kind? SelectedTournamentKind
    {
        get
        {
            if (this._selectedTournamentKind is null)
            {
                if (this.SetField(ref this._selectedTournamentKind, this.TournamentKinds.First()))
                {
                    this.OnPropertyChanged(nameof(this.VisibleIfTeamsAllowed));
                }
            }

            return this._selectedTournamentKind;
        }
        set
        {
            if (this.SetField(ref this._selectedTournamentKind, value))
            {
                this.OnPropertyChanged(nameof(this.VisibleIfTeamsAllowed));
            }
        }
    }

    public DataAccess.Entities.System? SelectedTournamentSystem
    {
        get
        {
            if (this._selectedTournamentSystem is null)
            {
                this.SetField(ref this._selectedTournamentSystem, this.TournamentSystems.First());
            }

            return this._selectedTournamentSystem;
        }
        set { this.SetField(ref this._selectedTournamentSystem, value); }
    }

    public int SelectedTournamentRoundsCount
    {
        get
        {
            if (this._selectedTournamentRoundsCount is null)
            {
                this.SetField(ref this._selectedTournamentRoundsCount, this.TournamentRoundsCountItems.First());
            }

            return (int)this._selectedTournamentRoundsCount;
        }
        set { this.SetField(ref this._selectedTournamentRoundsCount, value); }
    }

    public TimeOnly SelectedTime
    {
        get
        {
            if (this._selectedTime is null)
            {
                this.SetField(ref this._selectedTime, this.TimeItems.First());
            }

            return (TimeOnly)this._selectedTime;
        }
        set { this.SetField(ref this._selectedTime, value); }
    }

    public int SelectedDurationHours
    {
        get
        {
            if (this._selectedDurationHours is null)
            {
                this.SetField(ref this._selectedDurationHours, this.DurationHoursItems.First());
            }

            return (int)this._selectedDurationHours;
        }
        set { this.SetField(ref this._selectedDurationHours, value); }
    }

    public ObservableCollection<int> TeamPlayersCountItems
    {
        get
        {
            return new ObservableCollection<int>
                   {
                       2, 3, 4, 5, 6, 7, 8, 9, 10
                   };
        }
    }

    public int SelectedMaxTeamPlayers
    {
        get { return this._selectedMaxTeamPlayers; }
        set { this.SetField(ref this._selectedMaxTeamPlayers, value); }
    }

    public Visibility VisibleIfTeamsAllowed
    {
        get
        {
            if (this.SelectedTournamentKind is not null && this.SelectedTournamentKind.KindName.Contains("team"))
            {
                this.SetField(ref this._visibleIfTeamsAllowed, Visibility.Visible);
            }
            else
            {
                this.SetField(ref this._visibleIfTeamsAllowed, Visibility.Collapsed);
            }

            return (Visibility)this._visibleIfTeamsAllowed;
        }
        set { this.SetField(ref this._visibleIfTeamsAllowed, value); }
    }
}
