using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.EditGroup;

public partial class EditGroupWindow
{
    public EditGroupWindow()
    {
        this.InitializeComponent();
    }


    public EditGroupWindow(Group? group)
    {
        this.InitializeComponent();
        this.DataContext = new EditGroupViewModel(group);
    }
}
