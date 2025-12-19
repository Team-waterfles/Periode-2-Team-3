using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typ_IO.Core.Models
{
    public class LevelScore
    {
        public LevelScore(string naam, int topscore, int positie = 0)
        {
            Naam = naam;
            Positie = positie;
            TopScore = topscore;
        }
        public string Naam { get; set; }
        public int Positie { get; set; }
        public int TopScore { get; set; }
    }
}
