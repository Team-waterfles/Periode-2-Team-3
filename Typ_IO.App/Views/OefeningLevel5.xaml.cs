using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class OefeningLevel5 : ContentView
    {
        public OefeningLevel5()
        {
            InitializeComponent();

            // Level 5: Paragrafen - langere tekst met meerdere zinnen
            string voorbeeldTekst = "Dit is een uitgebreide typoefening voor gevorderde gebruikers. " +
                                   "Typen is een belangrijke vaardigheid in de moderne wereld. " +
                                   "Of je nu student bent, professional of hobbyist, goede typvaardigheden maken je werk een stuk efficiënter. " +
                                   "Door regelmatig te oefenen met verschillende soorten teksten, verbeter je niet alleen je snelheid maar ook je nauwkeurigheid. " +
                                   "Muziek en ritme kunnen helpen om je typritme te verbeteren en het oefenen aangenamer te maken.";

            BindingContext = new OefeningViewModel(
                this.Dispatcher,
                voorbeeldTekst
            );
        }
    }
}