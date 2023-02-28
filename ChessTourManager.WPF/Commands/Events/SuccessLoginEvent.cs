using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Commands.Events;

public static class SuccessLoginEvent
{
    /// <summary>
    ///     Delegate on handling UserSuccessLogin event.
    /// </summary>
    public delegate void UserSuccessLoginHandler(SuccessLoginEventArgs e);

    public static event UserSuccessLoginHandler? UserSuccessLogin;
    internal static void OnUserSuccessLogin(SuccessLoginEventArgs e) => UserSuccessLogin?.Invoke(e);
}

public class SuccessLoginEventArgs : EventArgs
{
    public DateTimeOffset LoginDateTimeUtc;
    public User?          User;

    public SuccessLoginEventArgs(User? user, DateTimeOffset loginDateTimeUtc)
    {
        User             = user;
        LoginDateTimeUtc = loginDateTimeUtc;
    }
}
