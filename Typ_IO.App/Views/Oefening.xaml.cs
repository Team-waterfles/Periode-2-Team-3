using BasisJaar2.ViewModels;

namespace BasisJaar2.Views;

public partial class Oefening : ContentView
{
    private OefeningViewModel VM => BindingContext as OefeningViewModel;

    public Oefening()
    {
        InitializeComponent();

        string voorbeeldTekst = "De snelle bruine vos springt over de luie hond. Typ dit nauwkeurig!";

        BindingContext = new OefeningViewModel(this.Dispatcher, voorbeeldTekst);

        // Auto-focus op de onzichtbare Editor
        Dispatcher.Dispatch(async () =>
        {
            await Task.Delay(50); // korte delay om layout te voltooien
            HiddenEditor.Focus();
        });
    }

    private void HiddenEditor_TextChanged(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (VM == null) return;

        string nieuweTekst = e.NewTextValue ?? string.Empty;

        // Auto-start bij eerste toetsaanslag
        if (!VM.Started && nieuweTekst.Length > 0)
        {
            VM.Start();
        }

        // Backspace
        if (nieuweTekst.Length < VM.Invoer.Length)
        {
            VM.VerwijderLaatste();
        }
        // Toevoegen karakter
        else if (nieuweTekst.Length > VM.Invoer.Length)
        {
            char toegevoegd = nieuweTekst[nieuweTekst.Length - 1];
            VM.VoegKarakterToe(toegevoegd);
        }

        // Reset editor zodat user nooit extra kan typen
        ((Editor)sender).Text = VM.Invoer;
    }

    private void Start_Clicked(object sender, EventArgs e) => VM?.Start();
    private void Stop_Clicked(object sender, EventArgs e) => VM?.Stop();
    private void Opnieuw_Clicked(object sender, EventArgs e) => VM?.Opnieuw();
    private void Terug_Clicked(object sender, EventArgs e) => VM?.Terug();
}