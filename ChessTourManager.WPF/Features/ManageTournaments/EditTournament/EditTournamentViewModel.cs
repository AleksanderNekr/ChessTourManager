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

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public class EditTournamentViewModel : ViewModelBase
{
    private static readonly ChessTourContext
        EditTournamentContext = MainViewModel.MainContext;

    private ApplyEditTournamentCommand? _applyEditTournamentCommand;

    private int _selectedMaxTeamPlayers = 4;

    private ObservableCollection<Kind>?                       _tournamentKinds;
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

            IGetQueries.CreateInstance(EditTournamentContext).GetKinds(out List<Kind>? kinds);
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

            IGetQueries.CreateInstance(EditTournamentContext)
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
        get { return EditingTournament?.TournamentName ?? "Название турнира"; }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.TournamentName = value;
                this.OnPropertyChanged();
            }
        }
    }


    public string? TournamentPlaceText
    {
        get { return EditingTournament?.Place ?? "Место проведения"; }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.Place = value;
                this.OnPropertyChanged();
            }
        }
    }

    public DateTime SelectedDate
    {
        get { return EditingTournament?.DateStart.ToDateTime(new TimeOnly(0, 0)) ?? DateTime.Now; }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.DateStart = DateOnly.FromDateTime(value);
                this.OnPropertyChanged();
            }
        }
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
        get { return EditingTournament?.OrganizationName ?? "Организатор"; }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.OrganizationName = value;
                this.OnPropertyChanged();
            }
        }
    }

    public bool IsMixedGroupsAllowed { get; set; } = true;

    public Kind SelectedTournamentKind
    {
        get { return EditingTournament?.Kind ?? this.TournamentKinds.First(); }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.Kind = value;
                this.OnPropertyChanged();
            }
        }
    }

    public DataAccess.Entities.System SelectedTournamentSystem
    {
        get { return EditingTournament?.System ?? this.TournamentSystems.First(); }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.System = value;
                this.OnPropertyChanged();
            }
        }
    }

    public int SelectedTournamentRoundsCount
    {
        get { return EditingTournament?.ToursCount ?? this.TournamentRoundsCountItems.First(); }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.ToursCount = value;
                this.OnPropertyChanged();
            }
        }
    }

    public TimeOnly SelectedTime
    {
        get { return EditingTournament?.TimeStart ?? this.TimeItems.First(); }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.TimeStart = value;
                this.OnPropertyChanged();
            }
        }
    }

    public int SelectedDurationHours
    {
        get { return EditingTournament?.Duration ?? this.DurationHoursItems.First(); }
        set
        {
            if (EditingTournament is not null)
            {
                EditingTournament.Duration = value;
                this.OnPropertyChanged();
            }
        }
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
            if (this.SelectedTournamentKind.Name.Contains("team"))
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

    public ICommand ApplyEditTournamentCommand
    {
        get { return this._applyEditTournamentCommand ??= new ApplyEditTournamentCommand(); }
    }

    public static Tournament? EditingTournament { get; set; }
}
