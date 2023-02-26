using System.Collections.ObjectModel;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.ViewModels;

public class PlayersViewModel : ViewModelBase
{
    private ObservableCollection<Player>? _playersCollection;
}
