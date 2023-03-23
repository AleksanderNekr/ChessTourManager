using System.Windows.Input;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageAccount.ChangePassword;

public class ChangePasswordViewModel : ViewModelBase
{
    private CompleteChangePasswordCommand? _completeChangePasswordCommand;

    private string? _currentPassword;
    private string? _newPassword;
    private string? _repeatPassword;

    public string CurrentPassword
    {
        get { return _currentPassword ?? string.Empty; }
        set { SetField(ref _currentPassword, value); }
    }

    public string NewPassword
    {
        get { return _newPassword ?? string.Empty; }
        set { SetField(ref _newPassword, value); }
    }

    public string RepeatPassword
    {
        get { return _repeatPassword ?? string.Empty; }
        set { SetField(ref _repeatPassword, value); }
    }

    public ICommand CompleteChangePasswordCommand
    {
        get { return _completeChangePasswordCommand ??= new CompleteChangePasswordCommand(this); }
    }
}
