namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;

public partial class AddGroupWindow
{
    public AddGroupWindow()
    {
        this.InitializeComponent();
        GroupAddedEvent.GroupAdded += this.GroupAddedEvent_GroupAdded;
    }

    private void GroupAddedEvent_GroupAdded(object source, GroupAddedEventArgs groupAddedEventArgs)
    {
        GroupAddedEvent.GroupAdded -= this.GroupAddedEvent_GroupAdded;
        this.Close();
    }
}
