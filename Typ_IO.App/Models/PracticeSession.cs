using System.Collections.Generic;
using Typ_IO.Core.Models;

namespace BasisJaar2.Models;

public static class PracticeSession
{
    public static Level? GeselecteerdLevel { get; set; }
    private static int Voortgang = 0;

    public static bool IsLevelUnlocked(int levelNummer)
    {
        if (levelNummer <= Voortgang + 1) return true;
        return false;
    }

    public static void MarkLevelGehaald(int levelNummer)
    {
        if (Voortgang < levelNummer)
        { Voortgang = levelNummer; }
    }

    public static bool IsLevelCompleted(int levelNummer)
    {
        return Voortgang >= levelNummer;
    }
}
