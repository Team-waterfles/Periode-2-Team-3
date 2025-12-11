using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typ_IO.Core.Services
{
    public static class Levelgenerator
    {
        public static string MaakLevelBijLetteropties(string letteropties, int levellengte)
        {
            string tekst = "";
            Random letterindex = new ();
            for (int i = 0; i < levellengte; i++)
            {
                tekst += letteropties[letterindex.Next(0, letteropties.Length)];
            }
            return tekst;
        }
    }
}
