using BasisJaar2.ViewModels;

namespace BasisJaar2.Views;

public partial class Oefening : ContentView
{
    public Oefening()
    {
        InitializeComponent();

        // De voorbeeldtekst die eerst in de Label stond
        string voorbeeldTekst =
            "De snelle bruine vos springt over de luie hond. " +
            "Dit is een eenvoudige oefening om je typvaardigheid te verbeteren.";

        // ViewModel instellen
        BindingContext = new OefeningViewModel(
            this.Dispatcher,     // nodig voor timers
            voorbeeldTekst
        );
    }
}
