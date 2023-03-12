using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;
using ChessTourManager.WPF.Features.ManageTournaments.DeleteTournament;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.DeletePlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.EditPlayer;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.AddTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.DeleteTeam;
using ChessTourManager.WPF.Features.ManageTournaments.ManageTeams.EditTeam;

namespace ChessTourManager.WPF.Features.ManageTournaments.Tree;

public partial class TreeControl : UserControl
{
    public TreeControl()
    {
        InitializeComponent();
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
                ((TreeViewModel)DataContext).SelectedTournament = tournament;
                break;
            case Team team:
                ((TreeViewModel)DataContext).SelectedTeam   = team;
                TournamentsListViewModel.SelectedTournament = team.Tournament;
                break;
            case Player player:
                ((TreeViewModel)DataContext).SelectedPlayer = player;
                TournamentsListViewModel.SelectedTournament = player.Team?.Tournament;
                break;
        }
    }

    private TreeViewItem? VisualUpwardSearch(DependencyObject? eOriginalSource)
    {
        while (eOriginalSource is not null && !(eOriginalSource is TreeViewItem))
        {
            eOriginalSource = VisualTreeHelper.GetParent(eOriginalSource);
        }

        return eOriginalSource as TreeViewItem;
    }

    private void EditTournamentMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        EditTournamentWindow editTournamentWindow = new(((TreeViewModel)DataContext).SelectedTournament);
        editTournamentWindow.ShowDialog();
    }

    private void AddTeamMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        TournamentsListViewModel.SelectedTournament = ((TreeViewModel)DataContext).SelectedTournament;
        AddTeamWindow addTeamWindow = new();
        addTeamWindow.ShowDialog();
    }

    private void AddTournamentMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new CreateTournamentWindow().ShowDialog();
    }

    private void DeleteTournamentMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new DeleteTournamentCommand(new TournamentsListViewModel())
           .Execute(((TreeViewModel)DataContext).SelectedTournament);
    }

    private void EditTeamMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new EditTeamWindow(((TreeViewModel)DataContext).SelectedTeam).ShowDialog();
    }

    private void DeleteTeamMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new DeleteTeamCommand()
           .Execute(((TreeViewModel)DataContext).SelectedTeam);
        ((TreeViewModel)DataContext).SelectedTeam = null;
    }

    private void EditPlayerMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new EditPlayerWindow(((TreeViewModel)DataContext).SelectedPlayer).ShowDialog();
    }

    private void AddPlayerMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new AddPlayerWindow(((TreeViewModel)DataContext).SelectedTeam).ShowDialog();
    }

    private void DeletePlayerMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new DeletePlayerCommand().Execute(((TreeViewModel)DataContext).SelectedPlayer);
        ((TreeViewModel)DataContext).SelectedPlayer = null;
    }
}
