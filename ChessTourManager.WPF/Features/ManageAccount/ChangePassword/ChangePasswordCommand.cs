﻿using ChessTourManager.WPF.Helpers;

namespace ChessTourManager.WPF.Features.ManageAccount.ChangePassword;

public class ChangePasswordCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        new ChangePasswordWindow().ShowDialog();
    }
}
