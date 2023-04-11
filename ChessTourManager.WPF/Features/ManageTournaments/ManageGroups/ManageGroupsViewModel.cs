using System;
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

public class ManageGroupsViewModel : ViewModelBase, IDisposable
{
    public ManageGroupsViewModel()
    {
        Subscribe();
    }

    public ObservableCollection<Group>? GroupsWithPlayers
    {
        get { return _groupsWithPlayers; }
        private set { SetField(ref _groupsWithPlayers, value); }
    }

    public ICommand AddGroupCommand
    {
        get { return _addGroupCommand ??= new AddGroupCommand(); }
    }

    public string GroupName
    {
        get { return _groupName ?? string.Empty; }
        set { SetField(ref _groupName, value); }
    }

    public string GroupIdentifier
    {
        get { return _groupIdentifier ?? string.Empty; }
        set { SetField(ref _groupIdentifier, value); }
    }

    public ICommand CompleteAddGroup
    {
        get { return _completeAddGroup ??= new CompleteAddGroupCommand(this); }
    }

    public ICommand DeleteGroupCommand
    {
        get { return _deleteGroupCommand ??= new DeleteGroupCommand(); }
    }

    public ICommand EditGroupCommand
    {
        get { return _editGroupCommand ??= new EditGroupCommand(); }
    }

    public void Dispose()
    {
        Unsubscribe();
    }

    private string?                      _groupIdentifier;
    private string?                      _groupName;
    private ObservableCollection<Group>? _groupsWithPlayers;

    private AddGroupCommand?         _addGroupCommand;
    private CompleteAddGroupCommand? _completeAddGroup;
    private DeleteGroupCommand?      _deleteGroupCommand;
    private EditGroupCommand?        _editGroupCommand;

    private void GroupDeletedEvent_GroupDeleted(object source, GroupDeletedEventArgs groupDeletedEventArgs)
    {
        UpdateGroups();
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        UpdateGroups();
    }

    private void GroupAddedEvent_GroupAdded(object source, GroupAddedEventArgs groupAddedEventArgs)
    {
        UpdateGroups();
    }

    private void TournamentOpenedEvent_TournamentOpened(object                    source,
                                                        TournamentOpenedEventArgs tournamentOpenedEventArgs)
    {
        UpdateGroups();
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened += TournamentOpenedEvent_TournamentOpened;
        GroupAddedEvent.GroupAdded             += GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged         += GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted         += GroupDeletedEvent_GroupDeleted;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened -= TournamentOpenedEvent_TournamentOpened;
        GroupAddedEvent.GroupAdded             -= GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged         -= GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted         -= GroupDeletedEvent_GroupDeleted;
    }

    private void UpdateGroups()
    {
        if (MainViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersViewModel.PlayersContext)
                   .TryGetGroups(LoginViewModel.CurrentUser.UserId,
                                 MainViewModel.SelectedTournament.TournamentId,
                                 out List<Group>? groups);
        if (groups is { })
        {
            GroupsWithPlayers = new ObservableCollection<Group>(groups);
        }
    }
}
