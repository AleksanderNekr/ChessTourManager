using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ChessTourManagerWpf.Models;
using ChessTourManagerWpf.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChessTourManagerWpf.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private static readonly ChessTourContext ChessTourContext = new();

    [ObservableProperty] private ObservableCollection<User> _usersCollection =
        new(ChessTourContext.Users.Select(u => u));

    [RelayCommand]
    private void ShowPlayers()
    {
//        MessageBox.Show(ChessTourContext.CreateInstance().Users.Count().ToString());
//        chessTourContext.Users.Add(new User
//                                   {
//                                       Email          = "alex@mail.com",
//                                       UserLastname   = "Smith",
//                                       UserFirstname  = "Alex",
//                                       UserPatronymic = "Alexandrovich",
//                                       PassHash       = "oqwd12odowqpn3243lsd32"
//                                   });
//
//        chessTourContext.SaveChanges();

        ChessTourContext.SaveChanges();
        MessageBox.Show(string.Join(" ", ChessTourContext.Users.Select(u => u.UserLastname).ToList()));
    }
}
