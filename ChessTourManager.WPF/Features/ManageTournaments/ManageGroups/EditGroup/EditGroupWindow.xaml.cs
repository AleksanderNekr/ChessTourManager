using System.Windows;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public partial class EditGroupWindow : Window
{
    public EditGroupWindow()
    {
        InitializeComponent();
    }


    public EditGroupWindow(Group? group)
    {
        InitializeComponent();
        DataContext = new EditGroupViewModel(group);
    }
}
