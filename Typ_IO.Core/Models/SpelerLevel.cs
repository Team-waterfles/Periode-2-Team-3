using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typ_IO.Core.Models
{
    public class SpelerLevel
    {
        public SpelerLevel() {}
        public int SpelerId { get; set; }
        public int LevelId { get; set; }
        public int TopScore { get; set; }
    }
}
