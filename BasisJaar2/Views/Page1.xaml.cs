using System.Diagnostics;

namespace BasisJaar2.Views;

public partial class Page1 : ContentView
{
    private Stopwatch _stopwatch;
    private bool _timerLoopt;
    private string _voorbeeldTekst;

    public Page1()
    {
        InitializeComponent();
        _stopwatch = new Stopwatch();
        _voorbeeldTekst = voorbeeldTekstLabel.Text;
    }

    // Start knop geklikt
    private void OnStartGeklikt(object sender, EventArgs e)
    {
        // Start timer
        _stopwatch.Start();
        _timerLoopt = true;

        // Enable invoerveld
        invoerVeld.IsEnabled = true;
        invoerVeld.Focus();
        invoerVeld.Text = "";

        // Knoppen aanpassen
        startKnop.IsEnabled = false;
        stopKnop.IsEnabled = true;

        // Resultaat verbergen
        resultaatFrame.IsVisible = false;

        // Start timer update
        StartTimerUpdate();
    }

    // Stop knop geklikt
    private void OnStopGeklikt(object sender, EventArgs e)
    {
        StopOefening();
    }

    // Tekst gewijzigd in invoerveld
    private void OnTekstGewijzigd(object sender, TextChangedEventArgs e)
    {
        // Update aantal getypte karakters
        aantalKaraktersLabel.Text = invoerVeld.Text.Length.ToString();

        // Check of oefening compleet is
        if (invoerVeld.Text.Length >= _voorbeeldTekst.Length)
        {
            StopOefening();
        }
    }

    // Opnieuw knop geklikt
    private void OnOpnieuwGeklikt(object sender, EventArgs e)
    {
        // Reset alles
        _stopwatch.Reset();
        tijdLabel.Text = "00:00";
        aantalKaraktersLabel.Text = "0";
        invoerVeld.Text = "";
        invoerVeld.IsEnabled = false;

        startKnop.IsEnabled = true;
        stopKnop.IsEnabled = false; 
        resultaatFrame.IsVisible = false;
        _timerLoopt = false;
    }

    // Stop de oefening
    private void StopOefening()
    {
        _stopwatch.Stop();
        _timerLoopt = false;

        // Disable invoerveld
        invoerVeld.IsEnabled = false;

        // Knoppen aanpassen
        startKnop.IsEnabled = true;
        stopKnop.IsEnabled = false;

        // Bereken resultaten
        ToonResultaat();
    }

    // Toon resultaat
    private void ToonResultaat()
    {
        var tijd = _stopwatch.Elapsed;
        var aantalKarakters = invoerVeld.Text.Length;
        var aantalWoorden = TelWoorden(invoerVeld.Text);
        var tijdInMinuten = tijd.TotalMinutes;
        var wpm = tijdInMinuten > 0 ? Math.Round(aantalWoorden / tijdInMinuten, 2) : 0;

        // Toon resultaat
        resultaatTekstLabel.Text = $"Tijd: {tijd:mm\\:ss}\n" +
                                   $"Karakters: {aantalKarakters}\n" +
                                   $"Woorden: {aantalWoorden}\n" +
                                   $"WPM: {wpm}";

        resultaatFrame.IsVisible = true;
    }

    // Tel woorden in tekst
    private int TelWoorden(string tekst)
    {
        if (string.IsNullOrWhiteSpace(tekst))
            return 0;

        return tekst.Split(new[] { ' ', '\n', '\r', '\t' },
                          StringSplitOptions.RemoveEmptyEntries).Length;
    }

    // Update timer elke seconde
    private void StartTimerUpdate()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            if (_timerLoopt)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    tijdLabel.Text = _stopwatch.Elapsed.ToString(@"mm\:ss");
                });
                return true; // Blijf timer runnen
            }
            return false; // Stop timer
        });
    }
}