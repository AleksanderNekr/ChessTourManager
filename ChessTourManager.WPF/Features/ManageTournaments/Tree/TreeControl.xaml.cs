using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;
using ChessTourManager.WPF.Features.ManageTournaments.DeleteTournament;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

namespace ChessTourManager.WPF.Features.ManageTournaments.Tree;

public partial class TreeControl : IDisposable
{
    public TreeControl()
    {
        this.InitializeComponent();
    }

    private void TreeView_OnPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        TreeViewItem? item = VisualUpwardSearch(e.OriginalSource as DependencyObject);
        if (item == null)
        {
            return;
        }

        item.Focus();
        item.IsSelected = true;
        switch (item.DataContext)
        {
            case Tournament tournament:
                ((TreeViewModel)this.DataContext).SelectedTournament = tournament;
                break;
            case Team team:
                ((TreeViewModel)this.DataContext).SelectedTeam   = team;
                MainViewModel.SelectedTournament = team.Tournament;
                break;
            case Player player:
                ((TreeViewModel)this.DataContext).SelectedPlayer = player;
                MainViewModel.SelectedTournament = player.Team?.Tournament;
                break;
        }
    }

    private static TreeViewItem? VisualUpwardSearch(DependencyObject? eOriginalSource)
    {
        while (eOriginalSource is not null and not TreeViewItem)
        {
            eOriginalSource = VisualTreeHelper.GetParent(eOriginalSource);
        }

        return eOriginalSource as TreeViewItem;
    }

    private void EditTournamentMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        EditTournamentWindow editTournamentWindow = new(((TreeViewModel)this.DataContext).SelectedTournament);
        editTournamentWindow.ShowDialog();
    }

    private void AddTeamMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        MainViewModel.SelectedTournament = ((TreeViewModel)this.DataContext).SelectedTournament;
        AddTeamWindow addTeamWindow = new();
        addTeamWindow.ShowDialog();
    }

    private void AddTournamentMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new CreateTournamentWindow().ShowDialog();
    }

    private void DeleteTournamentMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new DeleteTournamentCommand(new MainViewModel())
           .Execute(((TreeViewModel)this.DataContext).SelectedTournament);
    }

    private void EditTeamMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new EditTeamWindow(((TreeViewModel)this.DataContext).SelectedTeam).ShowDialog();
    }

    private void DeleteTeamMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new DeleteTeamCommand()
           .Execute(((TreeViewModel)this.DataContext).SelectedTeam);
        ((TreeViewModel)this.DataContext).SelectedTeam = null;
    }

    private void EditPlayerMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new EditPlayerWindow(((TreeViewModel)this.DataContext).SelectedPlayer).ShowDialog();
    }

    private void AddPlayerMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new AddPlayerWindow(((TreeViewModel)this.DataContext).SelectedTeam).ShowDialog();
    }

    private void DeletePlayerMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new DeletePlayerCommand().Execute(((TreeViewModel)this.DataContext).SelectedPlayer);
        ((TreeViewModel)this.DataContext).SelectedPlayer = null;
    }

    public void Dispose()
    {
        ((IDisposable)this.DataContext).Dispose();
    }
}
