﻿using System;
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
            if (_tournamentKinds is { })
            {
                return _tournamentKinds;
            }

            IGetQueries.CreateInstance(CreateTournamentContext).GetKinds(out List<Kind>? kinds);
            if (kinds is { })
            {
                _tournamentKinds = new ObservableCollection<Kind>(kinds);
            }

            return _tournamentKinds ??= new ObservableCollection<Kind>();
        }
    }

    public ObservableCollection<DataAccess.Entities.System> TournamentSystems
    {
        get
        {
            if (_tournamentSystems is { })
            {
                return _tournamentSystems;
            }

            IGetQueries.CreateInstance(CreateTournamentContext)
                       .GetSystems(out List<DataAccess.Entities.System>? systems);
            if (systems is { })
            {
                _tournamentSystems = new ObservableCollection<DataAccess.Entities.System>(systems);
            }

            return _tournamentSystems ??= new ObservableCollection<DataAccess.Entities.System>();
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
            if (_tournamentNameText is null)
            {
                SetField(ref _tournamentNameText, string.Empty);
            }

            return _tournamentNameText;
        }
        set { SetField(ref _tournamentNameText, value); }
    }


    public string? TournamentPlaceText
    {
        get
        {
            if (_tournamentPlaceText is null)
            {
                SetField(ref _tournamentPlaceText, string.Empty);
            }

            return _tournamentPlaceText;
        }
        set { SetField(ref _tournamentPlaceText, value); }
    }

    public DateTime? SelectedDate
    {
        get
        {
            if (_selectedDate is null)
            {
                SetField(ref _selectedDate, DateTime.Now);
            }

            return (DateTime)_selectedDate;
        }
        set { SetField(ref _selectedDate, value); }
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
            if (_orgNameText is null)
            {
                SetField(ref _orgNameText, string.Empty);
            }

            return _orgNameText;
        }
        set { SetField(ref _orgNameText, value); }
    }

    public bool IsMixedGroupsAllowed { get; set; } = true;

    public ICommand CreateTournamentCommand
    {
        get { return _createTournamentCommand ??= new CreateTournamentCommand(this); }
    }

    public Kind? SelectedTournamentKind
    {
        get
        {
            if (_selectedTournamentKind is null)
            {
                if (SetField(ref _selectedTournamentKind, TournamentKinds.First()))
                {
                    OnPropertyChanged(nameof(VisibleIfTeamsAllowed));
                }
            }

            return _selectedTournamentKind;
        }
        set
        {
            if (SetField(ref _selectedTournamentKind, value))
            {
                OnPropertyChanged(nameof(VisibleIfTeamsAllowed));
            }
        }
    }

    public DataAccess.Entities.System? SelectedTournamentSystem
    {
        get
        {
            if (_selectedTournamentSystem is null)
            {
                SetField(ref _selectedTournamentSystem, TournamentSystems.First());
            }

            return _selectedTournamentSystem;
        }
        set { SetField(ref _selectedTournamentSystem, value); }
    }

    public int SelectedTournamentRoundsCount
    {
        get
        {
            if (_selectedTournamentRoundsCount is null)
            {
                SetField(ref _selectedTournamentRoundsCount, TournamentRoundsCountItems.First());
            }

            return (int)_selectedTournamentRoundsCount;
        }
        set { SetField(ref _selectedTournamentRoundsCount, value); }
    }

    public TimeOnly SelectedTime
    {
        get
        {
            if (_selectedTime is null)
            {
                SetField(ref _selectedTime, TimeItems.First());
            }

            return (TimeOnly)_selectedTime;
        }
        set { SetField(ref _selectedTime, value); }
    }

    public int SelectedDurationHours
    {
        get
        {
            if (_selectedDurationHours is null)
            {
                SetField(ref _selectedDurationHours, DurationHoursItems.First());
            }

            return (int)_selectedDurationHours;
        }
        set { SetField(ref _selectedDurationHours, value); }
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
        get { return _selectedMaxTeamPlayers; }
        set { SetField(ref _selectedMaxTeamPlayers, value); }
    }

    public Visibility VisibleIfTeamsAllowed
    {
        get
        {
            if (SelectedTournamentKind is { } && SelectedTournamentKind.KindName.Contains("team"))
            {
                SetField(ref _visibleIfTeamsAllowed, Visibility.Visible);
            }
            else
            {
                SetField(ref _visibleIfTeamsAllowed, Visibility.Collapsed);
            }

            return (Visibility)_visibleIfTeamsAllowed;
        }
        set { SetField(ref _visibleIfTeamsAllowed, value); }
    }
}
