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
        EditTournamentContext = TournamentsListViewModel.TournamentsListContext;

    private ApplyEditTournamentCommand? _applyEditTournamentCommand;

    private int _selectedMaxTeamPlayers = 4;

    private ObservableCollection<Kind>?                       _tournamentKinds;
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

            IGetQueries.CreateInstance(EditTournamentContext).GetKinds(out IEnumerable<Kind>? kinds);
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

            IGetQueries.CreateInstance(EditTournamentContext)
                       .GetSystems(out IEnumerable<DataAccess.Entities.System>? systems);
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
        get { return EditingTournament?.TournamentName ?? "Название турнира"; }
        set
        {
            if (EditingTournament is { })
            {
                EditingTournament.TournamentName = value;
                OnPropertyChanged();
            }
        }
    }


    public string? TournamentPlaceText
    {
        get { return EditingTournament?.Place ?? "Место проведения"; }
        set
        {
            if (EditingTournament is { })
            {
                EditingTournament.Place = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime SelectedDate
    {
        get { return EditingTournament?.DateStart.ToDateTime(new TimeOnly(0, 0)) ?? DateTime.Now; }
        set
        {
            if (EditingTournament is { })
            {
                EditingTournament.DateStart = DateOnly.FromDateTime(value);
                OnPropertyChanged();
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
            if (EditingTournament is { })
            {
                EditingTournament.OrganizationName = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsMixedGroupsAllowed { get; set; } = true;

    public Kind SelectedTournamentKind
    {
        get { return EditingTournament?.Kind ?? TournamentKinds.First(); }
        set
        {
            if (EditingTournament is { })
            {
                EditingTournament.Kind = value;
                OnPropertyChanged();
            }
        }
    }

    public DataAccess.Entities.System SelectedTournamentSystem
    {
        get { return EditingTournament?.System ?? TournamentSystems.First(); }
        set
        {
            if (EditingTournament is { })
            {
                EditingTournament.System = value;
                OnPropertyChanged();
            }
        }
    }

    public int SelectedTournamentRoundsCount
    {
        get { return EditingTournament?.ToursCount ?? TournamentRoundsCountItems.First(); }
        set
        {
            if (EditingTournament is { })
            {
                EditingTournament.ToursCount = value;
                OnPropertyChanged();
            }
        }
    }

    public TimeOnly SelectedTime
    {
        get { return EditingTournament?.TimeStart ?? TimeItems.First(); }
        set
        {
            if (EditingTournament is { })
            {
                EditingTournament.TimeStart = value;
                OnPropertyChanged();
            }
        }
    }

    public int SelectedDurationHours
    {
        get { return EditingTournament?.Duration ?? DurationHoursItems.First(); }
        set
        {
            if (EditingTournament is { })
            {
                EditingTournament.Duration = value;
                OnPropertyChanged();
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
        get { return _selectedMaxTeamPlayers; }
        set { SetField(ref _selectedMaxTeamPlayers, value); }
    }

    public Visibility VisibleIfTeamsAllowed
    {
        get
        {
            if (SelectedTournamentKind.KindName.Contains("team"))
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

    public ICommand ApplyEditTournamentCommand
    {
        get { return _applyEditTournamentCommand ??= new ApplyEditTournamentCommand(); }
    }

    public static Tournament? EditingTournament { get; set; }
}
