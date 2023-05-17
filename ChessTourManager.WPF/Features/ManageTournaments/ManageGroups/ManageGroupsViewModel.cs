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
        this.Subscribe();
    }

    public ObservableCollection<Group>? GroupsWithPlayers
    {
        get { return this._groupsWithPlayers; }
        private set { this.SetField(ref this._groupsWithPlayers, value); }
    }

    public ICommand AddGroupCommand
    {
        get { return this._addGroupCommand ??= new AddGroupCommand(); }
    }

    public string GroupName
    {
        get { return this._groupName ?? string.Empty; }
        set { this.SetField(ref this._groupName, value); }
    }

    public string GroupIdentifier
    {
        get { return this._groupIdentifier ?? string.Empty; }
        set { this.SetField(ref this._groupIdentifier, value); }
    }

    public ICommand CompleteAddGroup
    {
        get { return this._completeAddGroup ??= new CompleteAddGroupCommand(this); }
    }

    public ICommand DeleteGroupCommand
    {
        get { return this._deleteGroupCommand ??= new DeleteGroupCommand(); }
    }

    public ICommand EditGroupCommand
    {
        get { return this._editGroupCommand ??= new EditGroupCommand(); }
    }

    public void Dispose()
    {
        this.Unsubscribe();
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
        this.UpdateGroups();
    }

    private void GroupChangedEvent_GroupChanged(object source, GroupChangedEventArgs groupChangedEventArgs)
    {
        this.UpdateGroups();
    }

    private void GroupAddedEvent_GroupAdded(object source, GroupAddedEventArgs groupAddedEventArgs)
    {
        this.UpdateGroups();
    }

    private void TournamentOpenedEvent_TournamentOpened(object                    source,
                                                        TournamentOpenedEventArgs tournamentOpenedEventArgs)
    {
        this.UpdateGroups();
    }

    private void Subscribe()
    {
        TournamentOpenedEvent.TournamentOpened += this.TournamentOpenedEvent_TournamentOpened;
        GroupAddedEvent.GroupAdded             += this.GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged         += this.GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted         += this.GroupDeletedEvent_GroupDeleted;
    }

    private void Unsubscribe()
    {
        TournamentOpenedEvent.TournamentOpened -= this.TournamentOpenedEvent_TournamentOpened;
        GroupAddedEvent.GroupAdded             -= this.GroupAddedEvent_GroupAdded;
        GroupChangedEvent.GroupChanged         -= this.GroupChangedEvent_GroupChanged;
        GroupDeletedEvent.GroupDeleted         -= this.GroupDeletedEvent_GroupDeleted;
    }

    private void UpdateGroups()
    {
        if (MainViewModel.SelectedTournament is null || LoginViewModel.CurrentUser is null)
        {
            return;
        }

        IGetQueries.CreateInstance(PlayersViewModel.PlayersContext)
                   .TryGetGroups(LoginViewModel.CurrentUser.Id,
                                 MainViewModel.SelectedTournament.Id,
                                 out List<Group>? groups);
        if (groups is not null)
        {
            this.GroupsWithPlayers = new ObservableCollection<Group>(groups);
        }
    }
}
