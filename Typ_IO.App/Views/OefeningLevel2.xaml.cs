using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class OefeningLevel2 : ContentView
    {
        public OefeningLevel2()
        {
            InitializeComponent();

            // Level 2: Uitgebreid - meer letters en combinaties
            string voorbeeldTekst = "qwer asdf zxcv uiop jkl qwer asdf zxcv " +
                                   "typen leren is leuk en nuttig voor iedereen " +
                                   "qwer asdf zxcv uiop jkl typen leren oefening";

            BindingContext = new OefeningViewModel(
                this.Dispatcher,
                voorbeeldTekst
            );
        }
    }
}