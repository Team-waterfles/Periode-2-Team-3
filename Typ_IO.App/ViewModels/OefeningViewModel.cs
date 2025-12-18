using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Controls;
using BasisJaar2.Models;
using BasisJaar2.Views;
using Typ_IO.Core.Models;
using Typ_IO.Core.Services;

namespace BasisJaar2.ViewModels
{
    public class OefeningViewModel : INotifyPropertyChanged
    {
        private readonly IDispatcher _dispatcher;
        private readonly Stopwatch _stopwatch;
        private bool _timerLoopt;
        private int _firstErrorIndex = -1;
        private LeaderboardService _leaderboardService;

    private List<char> _fouten = new List<char>();
        public IReadOnlyList<char> Fouten => _fouten.AsReadOnly();

        private int _totaalFouten;
        public int TotaalFouten
        {
            get => _totaalFouten;
            set { _totaalFouten = value; OnPropertyChanged(nameof(TotaalFouten)); }
        }

        public string VoorbeeldTekst { get; }
        public int SpelerId { get; }
        public int LevelId { get; }
        public bool IsOefening { get; }


        public bool PracticeModeHints { get; set; } = false;

        private string _huidigeHint;
        public string HuidigeHint
        {
            get => _huidigeHint;
            set { _huidigeHint = value; OnPropertyChanged(nameof(HuidigeHint)); }
        }

        private readonly Dictionary<char, string> _vingerHints = new()
    {
        { 'f', "Linker wijsvinger (F)" }, { 'F', "Linker wijsvinger (F)" },
        { 'j', "Rechter wijsvinger (J)" }, { 'J', "Rechter wijsvinger (J)" },
        { 'd', "Linker middelvinger (D)" }, { 'D', "Linker middelvinger (D)" },
        { 'k', "Rechter middelvinger (K)" }, { 'K', "Rechter middelvinger (K)" },
        { 's', "Linker ringvinger (S)" }, { 'S', "Linker ringvinger (S)" },
        { 'l', "Rechter ringvinger (L)" }, { 'L', "Rechter ringvinger (L)" },
        { 'a', "Linker pink (A)" }, { 'A', "Linker pink (A)" },
        { ';', "Rechter pink (;)" },
        { 'g', "Linker wijsvinger (G)" }, { 'G', "Linker wijsvinger (G)" },
        { 'h', "Rechter wijsvinger (H)" }, { 'H', "Rechter wijsvinger (H)" },
        { 't', "Linker wijsvinger (T)" }, { 'T', "Linker wijsvinger (T)" },
        { 'y', "Rechter wijsvinger (Y)" }, { 'Y', "Rechter wijsvinger (Y)" },
        { 'r', "Linker wijsvinger (R)" }, { 'R', "Linker wijsvinger (R)" },
        { 'u', "Rechter wijsvinger (U)" }, { 'U', "Rechter wijsvinger (U)" },
        { 'e', "Linker middelvinger (E)" }, { 'E', "Linker middelvinger (E)" },
        { 'i', "Rechter middelvinger (I)" }, { 'I', "Rechter middelvinger (I)" },
        { 'w', "Linker ringvinger (W)" }, { 'W', "Linker ringvinger (W)" },
        { 'o', "Rechter ringvinger (O)" }, { 'O', "Rechter ringvinger (O)" },
        { 'q', "Linker pink (Q)" }, { 'Q', "Linker pink (Q)" },
        { 'p', "Rechter pink (P)" }, { 'P', "Rechter pink (P)" },
        { 'z', "Linker ringvinger (Z)" }, { 'Z', "Linker ringvinger (Z)" },
        { 'x', "Linker middelvinger (X)" }, { 'X', "Linker middelvinger (X)" },
        { 'c', "Linker middelvinger (C)" }, { 'C', "Linker middelvinger (C)" },
        { 'v', "Linker wijsvinger (V)" }, { 'V', "Linker wijsvinger (V)" },
        { 'b', "Linker wijsvinger / duim (B)" }, { 'B', "Linker wijsvinger / duim (B)" },
        { 'n', "Rechter wijsvinger (N)" }, { 'N', "Rechter wijsvinger (N)" },
        { 'm', "Rechter middelvinger (M)" }, { 'M', "Rechter middelvinger (M)" },
        { ' ', "Spatiebalk (duim)" }
    };

        public OefeningViewModel(IDispatcher dispatcher, Level level, bool is_oefening)
        {
            _leaderboardService = new LeaderboardService();

            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            VoorbeeldTekst = level.Tekst;
            SpelerId = 1;
            LevelId = level.Id;
            IsOefening = is_oefening;

            _stopwatch = new Stopwatch();
            Invoer = string.Empty;
            UpdateFormattedInvoer();

            Tijd = "00:00";
            AantalKarakters = 0;
            StartEnabled = true;
            StopEnabled = false;
            ResultaatVisible = false;
            Started = false;

            UpdateHint(); // eerste hint tonen
        }

        public bool Started { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string naam) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));

        private string _invoer = string.Empty;
        public string Invoer
        {
            get => _invoer;
            set { _invoer = value; OnPropertyChanged(nameof(Invoer)); UpdateHint(); }
        }

        private int _aantalKarakters;
        public int AantalKarakters
        {
            get => _aantalKarakters;
            set { _aantalKarakters = value; OnPropertyChanged(nameof(AantalKarakters)); }
        }

        private string _tijd;
        public string Tijd
        {
            get => _tijd;
            set { _tijd = value; OnPropertyChanged(nameof(Tijd)); }
        }

        private bool _startEnabled;
        public bool StartEnabled
        {
            get => _startEnabled;
            set { _startEnabled = value; OnPropertyChanged(nameof(StartEnabled)); }
        }

        private bool _stopEnabled;
        public bool StopEnabled
        {
            get => _stopEnabled;
            set { _stopEnabled = value; OnPropertyChanged(nameof(StopEnabled)); }
        }

        private bool _resultaatVisible;
        public bool ResultaatVisible
        {
            get => _resultaatVisible;
            set { _resultaatVisible = value; OnPropertyChanged(nameof(ResultaatVisible)); }
        }

        private string _resultaatTekst;
        public string ResultaatTekst
        {
            get => _resultaatTekst;
            set { _resultaatTekst = value; OnPropertyChanged(nameof(ResultaatTekst)); }
        }

        private FormattedString _formattedInvoer;
        public FormattedString FormattedInvoer
        {
            get => _formattedInvoer;
            set { _formattedInvoer = value; OnPropertyChanged(nameof(FormattedInvoer)); }
        }

        public void VoegKarakterToe(char c)
        {
            if (_firstErrorIndex != -1) return;
            if (Invoer.Length >= VoorbeeldTekst.Length) return;

            if (c != VoorbeeldTekst[Invoer.Length])
            {
                _firstErrorIndex = Invoer.Length;
                _fouten.Add(c);
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
                    span.Text = Invoer[i] == ' ' ? "_" : Invoer[i].ToString();
                    span.TextColor = Colors.Red;
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

        private void UpdateHint()
        {
            if (!PracticeModeHints || Invoer.Length >= VoorbeeldTekst.Length)
            {
                HuidigeHint = string.Empty;
                return;
            }

            char volgende = VoorbeeldTekst[Invoer.Length];
            HuidigeHint = _vingerHints.ContainsKey(volgende) ? _vingerHints[volgende] : "Gebruik de juiste vinger voor deze toets";
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
                MainPageViewModel.Current.SubpageContent = new Views.LevelSelectie(1);

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

            bool foutenOk = _fouten.Count <= (AantalKarakters / 100.0) * 10;
            bool wpmOk = wpm >= 20;
            bool levelGehaald = foutenOk && wpmOk;

            if (levelGehaald)
            {
                PracticeSession.MarkLevelGehaald(LevelId);
                if (!IsOefening) { _leaderboardService.plaats_score(LevelId, SpelerId, 900); }
                ResultaatTekst += "\nLevel gehaald!";
            }
            else
            {
                ResultaatTekst += "\nLevel NIET gehaald.";
            }

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
}
