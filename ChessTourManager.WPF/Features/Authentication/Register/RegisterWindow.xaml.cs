using System.Windows;
using ChessTourManager.WPF.Features.ManageTournaments;

namespace ChessTourManager.WPF.Features.Authentication.Register;

/// <summary>
///     Interaction logic for RegisterWindow.xaml
/// </summary>
public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
    }

    private void CompleteRegisterButton_Click(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}
