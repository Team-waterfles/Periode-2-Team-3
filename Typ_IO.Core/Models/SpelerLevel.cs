using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typ_IO.Core.Models
{
    public class SpelerLevel(int speler_id, int level_id, int top_score)
    {
        public int SpelerId { get; set; } = speler_id;
        public int LevelId { get; set; } = level_id;
        public int TopScore { get; set; } = top_score;
    }
}
