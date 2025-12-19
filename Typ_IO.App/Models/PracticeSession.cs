using Microsoft.Maui.Storage;
using BasisJaar2.Models;

namespace Typ_IO.Core.Models
{
    public static class PracticeSession
    {
        public static Level GeselecteerdLevel { get; set; }

        private const string CompletedPrefix = "level_completed_";

        public static bool IsLevelCompleted(int levelId)
            => Preferences.Get(CompletedPrefix + levelId, false);

        // ✅ 1-based: alleen level 1 open, rest pas als vorige gehaald is
        public static bool IsLevelUnlocked(int levelId)
        {
            if (levelId <= 1) return true;
            return IsLevelCompleted(levelId - 1);
        }

        public static void MarkLevelGehaald(int levelId)
        {
            Preferences.Set(CompletedPrefix + levelId, true);
        }

        public static void ResetProgress(int maxLevelId = 50)
        {
            for (int i = 1; i <= maxLevelId; i++)
                Preferences.Remove(CompletedPrefix + i);
        }
    }
}
