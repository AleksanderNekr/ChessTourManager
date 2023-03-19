using System.Windows.Input;
using ChessTourManager.DataAccess;
using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.Authentication.Register;

public class RegisterViewModel : ViewModelBase
{
    private ICommand? _registerCommand;

    public string? LastName        { get; set; }
    public string? FirstName       { get; set; }
    public string? Patronymic      { get; set; }
    public string? Email           { get; set; }
    public string? PasswordInit    { get; set; }
    public string? PasswordConfirm { get; set; }

    public ICommand RegisterCommand
    {
        get { return _registerCommand ??= new RegisterCommand(this); }
    }

    internal static readonly ChessTourContext RegisterContext = new();
}
