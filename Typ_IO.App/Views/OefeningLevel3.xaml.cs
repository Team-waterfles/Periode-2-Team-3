using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class OefeningLevel3 : ContentView
    {
        public OefeningLevel3()
        {
            InitializeComponent();

            // Level 3: Woorden - complete Nederlandse woorden
            string voorbeeldTekst = "hallo wereld auto fiets computer toetsenbord " +
                                   "oefen typen leren school student programmeren " +
                                   "muziek ritme spelen piano gitaar drums zingen " +
                                   "Nederland Amsterdam Rotterdam Utrecht Groningen";

            BindingContext = new OefeningViewModel(
                this.Dispatcher,
                voorbeeldTekst
            );
        }
    }
}