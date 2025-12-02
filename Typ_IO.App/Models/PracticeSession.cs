using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisJaar2.Models
{
    public static class PracticeSession
    {
        public static Level? GeselecteerdLevel { get; set; }

        // Gehaalde levels bijhouden op basis van levelnummer
        private static readonly HashSet<int> GehaaldeLevels = new();

        // Level 1 is altijd unlocked
        public static bool IsLevelUnlocked(int levelNummer)
        {
            if (levelNummer <= 1)
                return true;

            // level n is unlocked als n-1 gehaald is
            return GehaaldeLevels.Contains(levelNummer - 1);
        }

        public static void MarkLevelGehaald(int levelNummer)
        {
            if (!GehaaldeLevels.Contains(levelNummer))
                GehaaldeLevels.Add(levelNummer);
        }
    }
}
