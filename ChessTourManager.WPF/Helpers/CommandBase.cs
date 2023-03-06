using System;
using System.Windows.Input;

namespace ChessTourManager.WPF.Helpers;

public abstract class CommandBase : ICommand
{
    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    public abstract void Execute(object? parameter);

    public event EventHandler? CanExecuteChanged;

    protected void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
