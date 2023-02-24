using System.Collections.ObjectModel;
using ChessTourManager.DataAccess.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WPF.ViewModels;

public partial class PlayersViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Player>? _playersCollection;

    public PlayersViewModel()
    {
        MainViewModel.ChessTourContext.Players.Load();
        PlayersCollection = MainViewModel.ChessTourContext.Players.Local.ToObservableCollection();
    }
}
