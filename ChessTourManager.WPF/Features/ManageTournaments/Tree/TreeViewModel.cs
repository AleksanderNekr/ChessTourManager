using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.Tree;

public class TreeViewModel : ViewModelBase
{
    private static readonly ChessTourContext TreeContext = new();

    private ObservableCollection<Tournament>? _tournaments;

    public ObservableCollection<Tournament> TournamentsRoot
    {
        get
        {
            if (_tournaments is not null || LoginViewModel.CurrentUser == null)
            {
                return _tournaments!;
            }

            IGetQueries.CreateInstance(TreeContext)
                       .TryGetTournamentsWithTeamsAndPlayers(LoginViewModel.CurrentUser.UserId,
                                                             out IEnumerable<Tournament>? tournaments);


            if (tournaments is not null)
            {
                SetField(ref _tournaments, new ObservableCollection<Tournament>(tournaments));
            }

            return _tournaments!;
        }
        set { SetField(ref _tournaments, value); }
    }
}
