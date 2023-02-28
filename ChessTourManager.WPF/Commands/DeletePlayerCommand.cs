using ChessTourManager.DataAccess.Entities;
using ChessTourManager.WPF.ViewModels;

namespace ChessTourManager.WPF.Commands;

public class DeletePlayerCommand : CommandBase
{
    private readonly PlayersViewModel _playersViewModel;

    public DeletePlayerCommand(PlayersViewModel playersViewModel)
    {
        _playersViewModel = playersViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is Player player)
        {
            
        }
    }
}
