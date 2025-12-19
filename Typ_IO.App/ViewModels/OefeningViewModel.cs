using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Controls;
using BasisJaar2.Models;
using Typ_IO.Core.Models;

namespace BasisJaar2.ViewModels
{
    public class OefeningViewModel : INotifyPropertyChanged
    {
        private readonly IDispatcher _dispatcher;
        private readonly Stopwatch _stopwatch;

        private bool _timerLoopt;
        private int _firstErrorIndex = -1;

        private readonly List<char> _fouten = new();
        public IReadOnlyList<char> Fouten => _fouten.AsReadOnly();

        private int _totaalFouten;
        public int TotaalFouten
        {
            get => _totaalFouten;
            set { _totaalFouten = value; OnPropertyChanged(nameof(TotaalFouten)); }
        }

        public string VoorbeeldTekst { get; }
        public int LevelId { get; }

        private string _huidigeHint = "";
        public string HuidigeHint
        {
            get => _huidigeHint;
            set { _huidigeHint = value; OnPropertyChanged(nameof(HuidigeHint)); }
        }

        private string _currentLetterImage;
        public string CurrentLetterImage
        {
            get => _currentLetterImage;
            set
            {
                _currentLetterImage = value;
                OnPropertyChanged(nameof(CurrentLetterImage));
                OnPropertyChanged(nameof(HasCurrentLetterImage));
            }
        }

        public bool HasCurrentLetterImage => !string.IsNullOrEmpty(CurrentLetterImage);

        private FormattedString _formattedVoorbeeldTekst;
        public FormattedString FormattedVoorbeeldTekst
        {
            get => _formattedVoorbeeldTekst;
            set { _formattedVoorbeeldTekst = value; OnPropertyChanged(nameof(FormattedVoorbeeldTekst)); }
        }

        private FormattedString _formattedInvoer;
        public FormattedString FormattedInvoer
        {
            get => _formattedInvoer;
            set { _formattedInvoer = value; OnPropertyChanged(nameof(FormattedInvoer)); }
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

        public OefeningViewModel(IDispatcher dispatcher, Level level)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            VoorbeeldTekst = level?.Tekst ?? "";
            LevelId = level?.Id ?? 1;

            _stopwatch = new Stopwatch();
            Invoer = string.Empty;

            Tijd = "00:00";
            AantalKarakters = 0;
            StartEnabled = true;
            StopEnabled = false;
            ResultaatVisible = false;
            Started = false;

            UpdateFormattedInvoer();
            UpdateHintAndImage();
        }

        public bool Started { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string naam) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));

        private string _invoer = "";
        public string Invoer
        {
            get => _invoer;
            set
            {
                _invoer = value ?? "";
                OnPropertyChanged(nameof(Invoer));
                UpdateHintAndImage();
            }
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

        private string _resultaatTekst = "";
        public string ResultaatTekst
        {
            get => _resultaatTekst;
            set { _resultaatTekst = value; OnPropertyChanged(nameof(ResultaatTekst)); }
        }

        public void VoegKarakterToe(char c)
        {
            if (_firstErrorIndex != -1) return;               // stop bij eerste fout (jouw keuze)
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
            UpdateHintAndImage();

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
            UpdateHintAndImage();
        }

        private void UpdateFormattedInvoer()
        {
            var invoerFs = new FormattedString();
            var voorbeeldFs = new FormattedString();

            for (int i = 0; i < VoorbeeldTekst.Length; i++)
            {
                var voorbeeldSpan = new Span
                {
                    Text = VoorbeeldTekst[i].ToString(),
                    TextColor = Colors.Black
                };

                if (i < Invoer.Length)
                {
                    if (_firstErrorIndex == i)
                        voorbeeldSpan.TextColor = Colors.Red;
                    else
                        voorbeeldSpan.TextColor = Colors.Gray;
                }

                voorbeeldFs.Spans.Add(voorbeeldSpan);

                if (i < Invoer.Length)
                {
                    var invoerSpan = new Span
                    {
                        Text = Invoer[i] == ' ' ? "_" : Invoer[i].ToString(),
                        TextColor = (i == _firstErrorIndex) ? Colors.Red : Colors.Black
                    };
                    invoerFs.Spans.Add(invoerSpan);
                }
            }

            FormattedVoorbeeldTekst = voorbeeldFs;
            FormattedInvoer = invoerFs;
        }

        private void UpdateHintAndImage()
        {
            if (string.IsNullOrEmpty(VoorbeeldTekst))
            {
                HuidigeHint = "";
                CurrentLetterImage = null;
                return;
            }

            int index = Invoer?.Length ?? 0;
            if (index >= VoorbeeldTekst.Length)
            {
                HuidigeHint = "";
                CurrentLetterImage = null;
                return;
            }

            char volgende = VoorbeeldTekst[index];

            // hint tekst
            HuidigeHint = _vingerHints.TryGetValue(volgende, out var hint)
                ? hint
                : "Gebruik de juiste vinger voor deze toets";

            // afbeelding: a.png..z.png + spatie.png
            char nextLower = char.ToLower(volgende);

            if (nextLower >= 'a' && nextLower <= 'z')
            {
                CurrentLetterImage = $"{nextLower}.png";
                return;
            }

            if (volgende == ' ')
            {
                CurrentLetterImage = "spatie.png";
                return;
            }

            if (volgende == ';')
            {
                // pas aan naar jouw bestandsnaam
                CurrentLetterImage = "puntkomma.png";
                return;
            }

            CurrentLetterImage = null;
        }

        public void Start()
        {
            if (Started) return;
            Started = true;

            _stopwatch.Reset();
            _stopwatch.Start();
            _timerLoopt = true;

            Invoer = "";
            _firstErrorIndex = -1;
            _fouten.Clear();
            TotaalFouten = 0;

            UpdateFormattedInvoer();

            StartEnabled = false;
            StopEnabled = true;
            ResultaatVisible = false;

            UpdateHintAndImage();
            StartTimerUpdate();
        }

        public void Stop() => StopOefening();

        public void Opnieuw()
        {
            _stopwatch.Reset();
            Tijd = "00:00";
            Invoer = "";
            _firstErrorIndex = -1;
            _fouten.Clear();
            TotaalFouten = 0;

            UpdateFormattedInvoer();

            StartEnabled = true;
            StopEnabled = false;
            ResultaatVisible = false;
            _timerLoopt = false;
            Started = false;

            UpdateHintAndImage();
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

            int totalTyped = Invoer?.Length ?? 0;
            int foutenAantal = _fouten.Count;

            double accuracy = totalTyped > 0
                ? ((totalTyped - foutenAantal) / (double)totalTyped) * 100.0
                : 0.0;

            ResultaatTekst =
                $"Tijd: {tijd:mm\\:ss}\nKarakters: {AantalKarakters}\nWoorden: {aantalWoorden}\nWPM: {wpm}\nAccuracy: {Math.Round(accuracy, 2)}%\nFouten: {foutenAantal}";

            bool volledigGetypt = totalTyped == VoorbeeldTekst.Length;
            bool accuracyOk = accuracy >= 90.0;
            bool wpmOk = wpm >= 20;

            bool levelGehaald = volledigGetypt && accuracyOk && wpmOk;

            if (levelGehaald)
            {
                PracticeSession.MarkLevelGehaald(LevelId);
                ResultaatTekst += "\nLevel gehaald!";
            }
            else
            {
                if (!volledigGetypt) ResultaatTekst += "\nNiet gehaald: tekst niet afgemaakt.";
                else if (!accuracyOk) ResultaatTekst += "\nNiet gehaald: accuracy minimaal 90%.";
                else if (!wpmOk) ResultaatTekst += "\nNiet gehaald: te weinig WPM.";
                else ResultaatTekst += "\nLevel NIET gehaald.";
            }

            ResultaatVisible = true;

            // hints uit aan het einde
            HuidigeHint = "";
            CurrentLetterImage = null;
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
