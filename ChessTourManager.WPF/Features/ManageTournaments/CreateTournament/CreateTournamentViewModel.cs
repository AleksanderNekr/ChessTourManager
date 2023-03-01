using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Get;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;

public class CreateTournamentViewModel : ViewModelBase
{
    private static readonly ChessTourContext CreateTournamentContext = new();

    private ObservableCollection<Kind>?                       _tournamentKinds;
    private ObservableCollection<DataAccess.Entities.System>? _tournamentSystems;

    private string?                  _tournamentNameText;
    private string?                  _tournamentPlaceText;
    private DateOnly                 _selectedDate;
    private string?                  _orgNameText;
    private CreateTournamentCommand? _createTournamentCommand;

    public ObservableCollection<Kind> TournamentKinds
    {
        get
        {
            if (_tournamentKinds != null)
            {
                return _tournamentKinds;
            }

            IGetQueries.CreateInstance(CreateTournamentContext).GetKinds(out IQueryable<Kind>? kinds);
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

            IGetQueries.CreateInstance(CreateTournamentContext)
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
        get
        {
            if (_tournamentNameText is null)
            {
                SetField(ref _tournamentNameText, "Название турнира");
            }

            return _tournamentNameText!;
        }
        set => SetField(ref _tournamentNameText, value);
    }


    public string TournamentPlaceText
    {
        get
        {
            if (_tournamentPlaceText is null)
            {
                SetField(ref _tournamentPlaceText, "Место проведения турнира");
            }

            return _tournamentPlaceText!;
        }
        set => SetField(ref _tournamentPlaceText, value);
    }

    public DateOnly MinDate => DateOnly.FromDateTime(DateTime.Now);

    public DateOnly SelectedDate
    {
        get
        {
            if (_selectedDate == default)
            {
                SetField(ref _selectedDate, DateOnly.FromDateTime(DateTime.Now));
            }

            return _selectedDate;
        }
        set => SetField(ref _selectedDate, value);
    }

    public ObservableCollection<string> HoursItems =>
        new()
        {
            "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00",
            "18:00", "19:00", "20:00", "21:00", "22:00", "23:00"
        };

    public ObservableCollection<int> DurationHoursItems =>
        new()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        };

    public string OrgNameText
    {
        get
        {
            if (_orgNameText is null)
            {
                SetField(ref _orgNameText, "Название организации");
            }

            return _orgNameText!;
        }
        set => SetField(ref _orgNameText, value);
    }

    public bool IsMixedGroupsAllowed { get; set; } = true;

    public ICommand CreateTournamentCommand => _createTournamentCommand ??= new CreateTournamentCommand(this);
}
