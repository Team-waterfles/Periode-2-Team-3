using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Controls;

namespace BasisJaar2.ViewModels;

public class OefeningViewModel : INotifyPropertyChanged
{
    private readonly IDispatcher _dispatcher;
    private readonly Stopwatch _stopwatch;
    private bool _timerLoopt;
    private int _firstErrorIndex = -1;

    private List<char> _fouten = new List<char>();
    public IReadOnlyList<char> Fouten => _fouten.AsReadOnly();

    private int _totaalFouten;
    public int TotaalFouten
    {
        get => _totaalFouten;
        set { _totaalFouten = value; OnPropertyChanged(nameof(TotaalFouten)); }
    }

    public string VoorbeeldTekst { get; }

    public OefeningViewModel(IDispatcher dispatcher, string voorbeeldTekst)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        VoorbeeldTekst = voorbeeldTekst ?? string.Empty;

        _stopwatch = new Stopwatch();

        Invoer = string.Empty;
        UpdateFormattedInvoer();

        Tijd = "00:00";
        AantalKarakters = 0;
        StartEnabled = true;
        StopEnabled = false;
        ResultaatVisible = false;
        Started = false;
    }

    public bool Started { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string naam) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));

    private string _invoer;
    public string Invoer { get => _invoer; set { _invoer = value; OnPropertyChanged(nameof(Invoer)); } }

    private int _aantalKarakters;
    public int AantalKarakters { get => _aantalKarakters; set { _aantalKarakters = value; OnPropertyChanged(nameof(AantalKarakters)); } }

    private string _tijd;
    public string Tijd { get => _tijd; set { _tijd = value; OnPropertyChanged(nameof(Tijd)); } }

    private bool _startEnabled;
    public bool StartEnabled { get => _startEnabled; set { _startEnabled = value; OnPropertyChanged(nameof(StartEnabled)); } }

    private bool _stopEnabled;
    public bool StopEnabled { get => _stopEnabled; set { _stopEnabled = value; OnPropertyChanged(nameof(StopEnabled)); } }

    private bool _resultaatVisible;
    public bool ResultaatVisible { get => _resultaatVisible; set { _resultaatVisible = value; OnPropertyChanged(nameof(ResultaatVisible)); } }

    private string _resultaatTekst;
    public string ResultaatTekst { get => _resultaatTekst; set { _resultaatTekst = value; OnPropertyChanged(nameof(ResultaatTekst)); } }

    private FormattedString _formattedInvoer;
    public FormattedString FormattedInvoer { get => _formattedInvoer; set { _formattedInvoer = value; OnPropertyChanged(nameof(FormattedInvoer)); } }

    public void VoegKarakterToe(char c)
    {
        if (_firstErrorIndex != -1) return; // Stop na fout totdat gecorrigeerd
        if (Invoer.Length >= VoorbeeldTekst.Length) return;

        if (c != VoorbeeldTekst[Invoer.Length])
        {
            _firstErrorIndex = Invoer.Length;
            _fouten.Add(c);       // registreer de fout
            TotaalFouten = _fouten.Count;
        }

        Invoer += c;
        AantalKarakters = Invoer.Length;
        UpdateFormattedInvoer();

        if (Invoer.Length == VoorbeeldTekst.Length && _firstErrorIndex == -1)
            StopOefening();
    }

    public void VerwijderLaatste()
    {
        if (Invoer.Length == 0) return;

        Invoer = Invoer.Substring(0, Invoer.Length - 1);

        // Als foutindex verwijderd wordt, reset deze
        if (_firstErrorIndex >= Invoer.Length)
            _firstErrorIndex = -1;

        UpdateFormattedInvoer();
    }

    private void UpdateFormattedInvoer()
    {
        var fs = new FormattedString();
        for (int i = 0; i < Invoer.Length; i++)
        {
            var span = new Span();

            if (i == _firstErrorIndex)
            {
                // Fout is een spatie → toon rode underscore
                if (Invoer[i] == ' ')
                {
                    span.Text = "_";
                    span.TextColor = Colors.Red;
                }
                else
                {
                    span.Text = Invoer[i].ToString();
                    span.TextColor = Colors.Red;
                }
            }
            else
            {
                span.Text = Invoer[i].ToString();
                span.TextColor = Colors.Black;
            }

            fs.Spans.Add(span);
        }

        FormattedInvoer = fs;
    }

    public void Start()
    {
        if (Started) return;
        Started = true;

        _stopwatch.Reset();
        _stopwatch.Start();
        _timerLoopt = true;

        Invoer = string.Empty;
        _firstErrorIndex = -1;
        UpdateFormattedInvoer();

        StartEnabled = false;
        StopEnabled = true;
        ResultaatVisible = false;

        StartTimerUpdate();
    }

    public void Stop() => StopOefening();

    public void Opnieuw()
    {
        _stopwatch.Reset();
        Tijd = "00:00";
        Invoer = string.Empty;
        _firstErrorIndex = -1;
        UpdateFormattedInvoer();

        StartEnabled = true;
        StopEnabled = false;
        ResultaatVisible = false;
        _timerLoopt = false;
        Started = false;

        _fouten.Clear();
        TotaalFouten = 0;
    }

    public void Terug()
    {
        Invoer = string.Empty;
        _firstErrorIndex = -1;
        UpdateFormattedInvoer();
        Started = false;

        if (MainPageViewModel.Current != null)
            MainPageViewModel.Current.SubpageContent = new Views.MoeilijkheidsgraadPage();
    }

    private void StopOefening()
    {
        if (!_timerLoopt) return;

        _stopwatch.Stop();
        _timerLoopt = false;
        StartEnabled = true;
        StopEnabled = false;

        ToonResultaat();
    }

    private void ToonResultaat()
    {
        var tijd = _stopwatch.Elapsed;
        var aantalWoorden = TelWoorden(Invoer);
        var tijdInMinuten = tijd.TotalMinutes;
        var wpm = tijdInMinuten > 0 ? Math.Round(aantalWoorden / tijdInMinuten, 2) : 0;

        string foutTekst = _fouten.Count > 0 ? $"Fouten ({_fouten.Count}): {string.Join(", ", _fouten)}" : "Geen fouten";

        ResultaatTekst =
            $"Tijd: {tijd:mm\\:ss}\nKarakters: {AantalKarakters}\nWoorden: {aantalWoorden}\nWPM: {wpm}\n{foutTekst}";

        ResultaatVisible = true;
    }

    private int TelWoorden(string tekst)
        => string.IsNullOrWhiteSpace(tekst) ? 0 :
           tekst.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

    private void StartTimerUpdate()
    {
        _dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            if (_timerLoopt)
            {
                Tijd = _stopwatch.Elapsed.ToString(@"mm\:ss");
                return true;
            }
            return false;
        });
    }
}