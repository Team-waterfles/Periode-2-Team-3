using System;

namespace Typ_IO.Core.Services
{
    public static class ScoreBerekening
    {
        // Deze functie berekent de score
        // Je geeft het: wpm (woorden per minuut), aantalFouten, aantalKarakters
        // Je krijgt terug: een score (getal)
        public static int BerekenScore(double wpm, int aantalFouten, int aantalKarakters)
        {
            // Stap 1: Bereken de basis score
            // Hoe sneller je typt, hoe hoger de basis score
            double basisScore = wpm * 100;

            // Stap 2: Bereken hoeveel procent fouten je maakte
            // Bijvoorbeeld: 5 fouten op 100 karakters = 5% = 0.05
            double foutPercentage = 0;
            if (aantalKarakters > 0)
            {
                foutPercentage = (double)aantalFouten / aantalKarakters;
            }

            // Stap 3: Bereken de straf voor fouten
            // Voor elke 1% fouten, verlies je 10% van je basis score
            // Bijvoorbeeld: 5% fouten = verlies 50% van je score
            double foutStraf = basisScore * (foutPercentage * 10);

            // Stap 4: Trek de straf af van de basis score
            double eindscore = basisScore - foutStraf;

            // Stap 5: Als de score negatief is, maak het 0
            if (eindscore < 0)
            {
                eindscore = 0;
            }

            // Stap 6: Rond af naar een heel getal en geef terug
            return (int)Math.Round(eindscore);
        }
    }
}