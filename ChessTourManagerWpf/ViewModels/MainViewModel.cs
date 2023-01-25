using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChessTourManagerWpf.ViewModels;

public class MainViewModel : ObservableObject
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(ClickCountsInfo))]
    private int _clickCounts;

    public string ClickCountsInfo => $"You clicked {_clickCounts} times";

    [RelayCommand]
    private void Click() => ClickCounts++;
}
