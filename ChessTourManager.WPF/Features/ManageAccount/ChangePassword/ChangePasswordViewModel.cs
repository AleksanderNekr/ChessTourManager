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
        get { return this._currentPassword ?? string.Empty; }
        set { this.SetField(ref this._currentPassword, value); }
    }

    public string NewPassword
    {
        get { return this._newPassword ?? string.Empty; }
        set { this.SetField(ref this._newPassword, value); }
    }

    public string RepeatPassword
    {
        get { return this._repeatPassword ?? string.Empty; }
        set { this.SetField(ref this._repeatPassword, value); }
    }

    public ICommand CompleteChangePasswordCommand
    {
        get { return this._completeChangePasswordCommand ??= new CompleteChangePasswordCommand(this); }
    }
}
