﻿using System.ComponentModel;
using System.Windows;
using ChessTourManager.WPF.Features.Authentication.Login;

namespace ChessTourManager.WPF.Features.Authentication.Register;

/// <inheritdoc cref="System.Windows.Window" />
public partial class RegisterWindow
{
    public RegisterWindow()
    {
        InitializeComponent();
        SuccessRegisterEvent.UserSuccessRegister += RegisterViewModel_UserSuccessRegister;
    }

    private void RegisterViewModel_UserSuccessRegister(object source, SuccessRegisterEventArgs successRegisterEventArgs)
    {
        new AuthWindow().Show();
        Close();
    }

    private void ReturnToAuthWindow_Click(object sender, RoutedEventArgs e)
    {
        new AuthWindow().Show();
        Close();
    }

    private void RegisterWindow_Closing(object? sender, CancelEventArgs e)
    {
        SuccessRegisterEvent.UserSuccessRegister -= RegisterViewModel_UserSuccessRegister;
    }
}
