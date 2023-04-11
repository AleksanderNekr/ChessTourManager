namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGroups.AddGroup;

public partial class AddGroupWindow
{
    public AddGroupWindow()
    {
        InitializeComponent();
        GroupAddedEvent.GroupAdded += GroupAddedEvent_GroupAdded;
    }

    private void GroupAddedEvent_GroupAdded(object source, GroupAddedEventArgs groupAddedEventArgs)
    {
        GroupAddedEvent.GroupAdded -= GroupAddedEvent_GroupAdded;
        Close();
    }
}
