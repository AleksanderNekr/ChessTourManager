using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ChessTourManagerWpf.Models;
using ChessTourManagerWpf.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChessTourManagerWpf.ViewModels;

public partial class TournamentsListViewModel : ObservableObject
{
    private static readonly ChessTourContext ChessTourContext = new();

    [ObservableProperty] private ObservableCollection<Tournament> _tournamentsCollection =
        new(ChessTourContext.Tournaments.Select(tournament => tournament));
}
