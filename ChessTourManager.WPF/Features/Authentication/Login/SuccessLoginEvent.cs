using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.Authentication.Login;

public static class SuccessLoginEvent
{
    /// <summary>
    ///     Delegate on handling UserSuccessLogin event.
    /// </summary>
    public delegate void UserSuccessLoginHandler(object source, SuccessLoginEventArgs e);

    public static event UserSuccessLoginHandler? UserSuccessLogin;

    internal static void OnUserSuccessLogin(object source, SuccessLoginEventArgs e)
    {
        UserSuccessLogin?.Invoke(source, e);
    }
}

public class SuccessLoginEventArgs : EventArgs
{
    public DateTimeOffset LoginDateTimeUtc;
    public User?          User;

    public SuccessLoginEventArgs(User? user, DateTimeOffset loginDateTimeUtc)
    {
        this.User          = user;
        this.LoginDateTimeUtc = loginDateTimeUtc;
    }
}
