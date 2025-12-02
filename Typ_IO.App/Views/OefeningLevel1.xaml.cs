using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class OefeningLevel1 : ContentView
    {
        public OefeningLevel1()
        {
            InitializeComponent();

            // Level 1: Basis letters - eenvoudige letter combinaties
            string voorbeeldTekst = "asdf jkl asdf jkl fff jjj aaa sss ddd kkk lll " +
                                   "asdf jkl asdf jkl fff jjj aaa sss ddd kkk lll " +
                                   "asdf jkl asdf jkl fff jjj aaa sss ddd kkk lll";

            BindingContext = new OefeningViewModel(
                this.Dispatcher,
                voorbeeldTekst
            );
        }
    }
}