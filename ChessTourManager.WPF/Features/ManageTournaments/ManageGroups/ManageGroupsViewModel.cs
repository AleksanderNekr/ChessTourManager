using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.WPF.Features.Authentication.Login;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.DeleteGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.OpenTournament;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups;

public class ManageGroupsViewModel : ViewModelBase
{
    internal static readonly ChessTourContext GroupsContext = PlayersViewModel.PlayersContext;
    private                  AddGroupCommand? _addGroupCommand;
    private                  string           _groupIdentifier;
    private                  string           _groupName;

    private ObservableCollection<Group> _groupsWithPlayers;

    public ManageGroupsViewModel()
    {
        CompleteAddGroup                       =  new CompleteAddGroupCommand(this);
        DeleteGroupCommand                     =  new DeleteGroupCommand();
        EditGroupCommand                       =  new EditGroupCommand(this);
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
    }

    public ObservableCollection<Group> GroupsWithPlayers
    {
        get { return _groupsWithPlayers; }
        private set { SetField(ref _groupsWithPlayers, value); }
    }

    public ICommand AddGroupCommand
    {
        get { return _addGroupCommand ??= new AddGroupCommand(this); }
    }

    public string GroupName
    {
        get { return _groupName; }
        set { SetField(ref _groupName, value); }
    }

    public string GroupIdentifier
    {
        get { return _groupIdentifier; }
        set { SetField(ref _groupIdentifier, value); }
    }

    public ICommand CompleteAddGroup   { get; }
    public ICommand DeleteGroupCommand { get; }
    public ICommand EditGroupCommand   { get; }

    private void GroupDeletedEvent_GroupDeleted(GroupDeletedEventArgs e)
    {
        UpdateGroups();
    }

    private void GroupChangedEvent_GroupChanged(GroupChangedEventArgs e)
    {
        UpdateGroups();
    }

    private void GroupAddedEvent_GroupAdded(GroupAddedEventArgs e)
    {
        UpdateGroups();
    }

    private void TournamentOpenedEvent_TournamentOpened(TournamentOpenedEventArgs e)
    {
        GroupAddedEvent.GroupAdded     += GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged += GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted += GroupDeletedEvent_GroupDeleted;
        UpdateGroups();
    }

    private void UpdateGroups()
    {
        IGetQueries.CreateInstance(GroupsContext)
                   .TryGetGroups(LoginViewModel.CurrentUser!.UserId,
                                 TournamentsListViewModel.SelectedTournament!.TournamentId,
                                 out IEnumerable<Group>? groups);
        if (groups is not null)
        {
            GroupsWithPlayers = new ObservableCollection<Group>(groups);
        }
    }
}
