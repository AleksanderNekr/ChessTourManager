using System;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManageGames.AddTour;

public class TourAddedEvent
{
    public delegate void TourAddedEventHandler(object sender, TourAddedEventArgs e);

    public static event TourAddedEventHandler? TourAdded;

    public static void OnTourAdded(object sender, TourAddedEventArgs e)
    {
        TourAdded?.Invoke(sender, e);
    }
}

public class TourAddedEventArgs : EventArgs
{
    public TourAddedEventArgs(int tourNumber)
    {
        TourNumber = tourNumber;
    }

    public int TourNumber { get; }
}
