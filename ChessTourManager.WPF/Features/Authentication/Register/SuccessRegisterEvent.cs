using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.Authentication.Register;

public static class SuccessRegisterEvent
{
    public delegate void UserSuccessRegisterHandler(object source, SuccessRegisterEventArgs e);

    public static event UserSuccessRegisterHandler? UserSuccessRegister;

    internal static void OnUserSuccessRegister(object source, SuccessRegisterEventArgs e)
    {
        UserSuccessRegister?.Invoke(source, e);
    }
}

public class SuccessRegisterEventArgs : EventArgs
{
    public DateTimeOffset RegisterDateTimeUtc;
    public User?          User;

    public SuccessRegisterEventArgs(User? user, DateTimeOffset registerDateTimeUtc)
    {
        User                = user;
        RegisterDateTimeUtc = registerDateTimeUtc;
    }
}
