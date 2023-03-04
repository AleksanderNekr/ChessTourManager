using System.Collections.ObjectModel;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChessTourManager.WPF.Features.ManageTournaments.Tree;

public class TreeViewModel : ViewModelBase
{
    private static readonly ChessTourContext TreeContext = new();

    private ObservableCollection<Tournament>? _tournaments;

    public ObservableCollection<Tournament> TournamentsRoot
    {
        get
        {
            if (_tournaments is null && LoginViewModel.CurrentUser != null)
            {
                IGetQueries.CreateInstance(TreeContext)
                           .TryGetTournamentsWithTeamsAndPlayers(LoginViewModel.CurrentUser.UserId,
                                                                 out IQueryable<Tournament>? tournaments);


                if (tournaments is not null)
                {
                    SetField(ref _tournaments, new(tournaments));
                }
            }

            return _tournaments!;
        }
        set { SetField(ref _tournaments, value); }
    }
}
