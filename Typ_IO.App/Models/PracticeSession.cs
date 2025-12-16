using System.Collections.Generic;

namespace BasisJaar2.Models;

public static class PracticeSession
{
    public static Level? GeselecteerdLevel { get; set; }
    private static readonly HashSet<int> GehaaldeLevels = new();

    public static bool IsLevelUnlocked(int levelNummer)
    {
        if (levelNummer <= 1) return true;
        return GehaaldeLevels.Contains(levelNummer - 1);
    }

    public static void MarkLevelGehaald(int levelNummer)
    {
        if (!GehaaldeLevels.Contains(levelNummer))
            GehaaldeLevels.Add(levelNummer);
    }

    public static bool IsLevelCompleted(int levelNummer)
    {
        return GehaaldeLevels.Contains(levelNummer);
    }
}
