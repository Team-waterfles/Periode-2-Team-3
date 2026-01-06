namespace Typ_IO.Core.Services
{
    public static class Levelgenerator
    {
        public static string MaakLevelBijLetteropties(string letteropties, int levellengte)
        {
            string tekst = "";
            Random letterindex = new();
            for (int i = 0; i < levellengte; i++)
            {
                tekst += letteropties[letterindex.Next(0, letteropties.Length)];
            }
            return tekst;
        }

        public static bool BevatAlleenToegestaneKarakters(string tekst, string toegestaneKarakters)
        {
            if (string.IsNullOrEmpty(tekst))
                return true;

            if (string.IsNullOrEmpty(toegestaneKarakters))
                return false;

            foreach (char c in tekst)
            {
                if (!toegestaneKarakters.Contains(c))
                    return false;
            }

            return true;
        }
    }
}