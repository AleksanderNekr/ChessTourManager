using System;
using ChessTourManager.DataAccess.Entities;

namespace ChessTourManager.WPF.Features.Authentication.Register;

public static class SuccessRegisterEvent
{
    public delegate void UserSuccessRegisterHandler(SuccessRegisterEventArgs e);

    public static event UserSuccessRegisterHandler? UserSuccessRegister;

    internal static void OnUserSuccessRegister(SuccessRegisterEventArgs e)
    {
        UserSuccessRegister?.Invoke(e);
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
