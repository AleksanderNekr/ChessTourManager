using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries;

namespace ChessTourManager.WPF.ViewModels;

public class PlayersViewModel : ViewModelBase
{
    private ObservableCollection<Player>? _playersCollection;

    public ObservableCollection<Player>? PlayersCollection
    {
        get
        {
            if (_playersCollection == null)
            {
                IGetQueries.CreateInstance().TryGetPlayers(2, 2, out IEnumerable<Player> players);
                _playersCollection = new ObservableCollection<Player>(players);
                OnPropertyChanged();
            }

            return _playersCollection;
        }
        set => SetField(ref _playersCollection, value);
    }
}
