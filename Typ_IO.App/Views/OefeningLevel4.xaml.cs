using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class OefeningLevel4 : ContentView
    {
        public OefeningLevel4()
        {
            InitializeComponent();

            // Level 4: Zinnen - complete zinnen met leestekens
            string voorbeeldTekst = "De kat zit op de mat. De hond rent door het park. " +
                                   "Typen leren is leuk en nuttig voor iedereen. " +
                                   "Muziek en ritme helpen bij het oefenen. " +
                                   "Practice makes perfect, dus blijf oefenen!";

            BindingContext = new OefeningViewModel(
                this.Dispatcher,
                voorbeeldTekst
            );
        }
    }
}