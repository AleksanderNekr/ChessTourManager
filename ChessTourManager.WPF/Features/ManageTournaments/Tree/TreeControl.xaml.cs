using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.Features.ManageTournaments.CreateTournament;
using ChessTourManager.WPF.Features.ManageTournaments.DeleteTournament;
using ChessTourManager.WPF.Features.ManageTournaments.EditTournament;
using ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers.AddPlayer;
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
        if (item?.DataContext is Tournament tournament)
        {
            item.Focus();
            item.IsSelected                                 = true;
            ((TreeViewModel)DataContext).SelectedTournament = tournament;
        }
        else if (item?.DataContext is Team team)
        {
            item.Focus();
            item.IsSelected                             = true;
            ((TreeViewModel)DataContext).SelectedTeam   = team;
            TournamentsListViewModel.SelectedTournament = team.Tournament;
        }
        else if (item?.DataContext is Player player)
        {
            item.Focus();
            item.IsSelected                             = true;
            ((TreeViewModel)DataContext).SelectedPlayer = player;
            TournamentsListViewModel.SelectedTournament = player.Team?.Tournament;
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
    }

    private void EditPlayerMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new EditPlayerWindow(((TreeViewModel)DataContext).SelectedPlayer).ShowDialog();
    }

    private void AddPlayerMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        new AddPlayerWindow(((TreeViewModel)DataContext).SelectedTeam).ShowDialog();
    }
}
