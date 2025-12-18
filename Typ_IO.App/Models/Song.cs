using System;

namespace BasisJaar2.Models;

public class Song
{
    public string Titel { get; set; }
    public string Artiest { get; set; }
    public string Moeilijkheid { get; set; }
    public int Bpm { get; set; }
    public TimeSpan Lengte { get; set; }

    // Voor UI weergave
    public string LengteFormatted => $"{(int)Lengte.TotalMinutes}:{Lengte.Seconds:D2}";
    public string BpmText => $"BPM {Bpm}";
    public string DifficultyInitial => Moeilijkheid.Substring(0, 1).ToUpper();
}