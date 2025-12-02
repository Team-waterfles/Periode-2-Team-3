using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisJaar2.Models
{
    public class Level
    {
        public int Nummer { get; set; }
        public string Naam { get; set; } = string.Empty;
        public string Beschrijving { get; set; } = string.Empty;
        public string Oefentekst { get; set; } = string.Empty;
    }
}
