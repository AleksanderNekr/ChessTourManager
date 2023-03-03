using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Get;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.EditTournament;

public class EditTournamentViewModel : ViewModelBase
{
    internal static readonly ChessTourContext            EditTournamentContext = new();
    private                  ApplyEditTournamentCommand? _applyEditTournamentCommand;

    private int _selectedMaxTeamPlayers = 4;

    private ObservableCollection<Kind>?                       _tournamentKinds;
    private ObservableCollection<DataAccess.Entities.System>? _tournamentSystems;
    private Visibility?                                       _visibleIfTeamsAllowed;

    public ObservableCollection<Kind> TournamentKinds
    {
        get
        {
            if (_tournamentKinds != null)
            {
                return _tournamentKinds;
            }

            IGetQueries.CreateInstance(EditTournamentContext).GetKinds(out IQueryable<Kind>? kinds);
            if (kinds != null)
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
            if (_tournamentSystems != null)
            {
                return _tournamentSystems;
            }

            IGetQueries.CreateInstance(EditTournamentContext)
                       .GetSystems(out IQueryable<DataAccess.Entities.System>? systems);
            if (systems != null)
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

    public string TournamentNameText
    {
        get => EditingTournament?.TournamentName ?? "Название турнира";
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.TournamentName = value;
                OnPropertyChanged();
            }
        }
    }


    public string TournamentPlaceText
    {
        get => EditingTournament?.Place ?? "Место проведения";
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.Place = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime SelectedDate
    {
        get => EditingTournament?.DateStart.ToDateTime(new TimeOnly(0, 0)) ?? DateTime.Now;
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.DateStart = DateOnly.FromDateTime(value);
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<TimeOnly> TimeItems =>
        new()
        {
            TimeOnly.FromDateTime(DateTime.Parse("07:00")),
            TimeOnly.FromDateTime(DateTime.Parse("08:00")),
            TimeOnly.FromDateTime(DateTime.Parse("09:00")),
            TimeOnly.FromDateTime(DateTime.Parse("10:00")),
            TimeOnly.FromDateTime(DateTime.Parse("11:00")),
            TimeOnly.FromDateTime(DateTime.Parse("12:00")),
            TimeOnly.FromDateTime(DateTime.Parse("13:00")),
            TimeOnly.FromDateTime(DateTime.Parse("14:00")),
            TimeOnly.FromDateTime(DateTime.Parse("15:00")),
            TimeOnly.FromDateTime(DateTime.Parse("16:00")),
            TimeOnly.FromDateTime(DateTime.Parse("17:00")),
            TimeOnly.FromDateTime(DateTime.Parse("18:00")),
            TimeOnly.FromDateTime(DateTime.Parse("19:00")),
            TimeOnly.FromDateTime(DateTime.Parse("20:00")),
            TimeOnly.FromDateTime(DateTime.Parse("21:00")),
            TimeOnly.FromDateTime(DateTime.Parse("22:00")),
            TimeOnly.FromDateTime(DateTime.Parse("23:00"))
        };

    public ObservableCollection<int> DurationHoursItems =>
        new()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        };

    public string OrgNameText
    {
        get => EditingTournament?.OrganizationName ?? "Организатор";
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.OrganizationName = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsMixedGroupsAllowed { get; set; } = true;

    public Kind SelectedTournamentKind
    {
        get => EditingTournament?.Kind ?? TournamentKinds.First();
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.Kind = value;
                OnPropertyChanged();
            }
        }
    }

    public DataAccess.Entities.System SelectedTournamentSystem
    {
        get => EditingTournament?.System ?? TournamentSystems.First();
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.System = value;
                OnPropertyChanged();
            }
        }
    }

    public int SelectedTournamentRoundsCount
    {
        get => EditingTournament?.ToursCount ?? TournamentRoundsCountItems.First();
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.ToursCount = value;
                OnPropertyChanged();
            }
        }
    }

    public TimeOnly SelectedTime
    {
        get => EditingTournament?.TimeStart ?? TimeItems.First();
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.TimeStart = value;
                OnPropertyChanged();
            }
        }
    }

    public int SelectedDurationHours
    {
        get => EditingTournament?.Duration ?? DurationHoursItems.First();
        set
        {
            if (EditingTournament != null)
            {
                EditingTournament.Duration = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<int> TeamPlayersCountItems =>
        new()
        {
            2, 3, 4, 5, 6, 7, 8, 9, 10
        };

    public int SelectedMaxTeamPlayers
    {
        get => _selectedMaxTeamPlayers;
        set => SetField(ref _selectedMaxTeamPlayers, value);
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

            return (Visibility)_visibleIfTeamsAllowed!;
        }
        set => SetField(ref _visibleIfTeamsAllowed, value);
    }

    public ICommand ApplyEditTournamentCommand => _applyEditTournamentCommand ??= new ApplyEditTournamentCommand();

    public static Tournament? EditingTournament { get; set; }
}
