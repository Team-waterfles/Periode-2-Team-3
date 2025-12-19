using BasisJaar2.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Typ_IO.Core.Models;
using Typ_IO.Core.Services;

namespace BasisJaar2.ViewModels
{
    public class OefeningViewModel : INotifyPropertyChanged
    {
        private readonly IDispatcher _dispatcher;
        private readonly Stopwatch _stopwatch = new();
        private readonly LeaderboardService _leaderboardService = new();

        private bool _timerLoopt;
        private int _firstErrorIndex = -1;

        private const int MaxKaraktersPerRegel = 40;
        private const int MinKaraktersLaatsteRegel = 15;

        private readonly List<string> _regels = new();
        private int _huidigeRegelIndex;

        private readonly List<(int Index, char Getypt, char Verwacht)> _fouten = new();

        public string VoorbeeldTekst { get; }
        public int SpelerId { get; }
        public int LevelId { get; }
        public bool IsOefening { get; }

        private int _aantalKarakters;
        public int AantalKarakters
        {
            get => _aantalKarakters;
            private set { _aantalKarakters = value; OnPropertyChanged(nameof(AantalKarakters)); }
        }

        private string _huidigeHint = "";
        public string HuidigeHint
        {
            get => _huidigeHint;
            private set { _huidigeHint = value; OnPropertyChanged(nameof(HuidigeHint)); }
        }

        private string _currentLetterImage;
        public string CurrentLetterImage
        {
            get => _currentLetterImage;
            private set
            {
                _currentLetterImage = value;
                OnPropertyChanged(nameof(CurrentLetterImage));
                OnPropertyChanged(nameof(HasCurrentLetterImage));
            }
        }

        public bool HasCurrentLetterImage => !string.IsNullOrEmpty(CurrentLetterImage);

        private string _invoer = string.Empty;
        public string Invoer
        {
            get => _invoer;
            private set
            {
                _invoer = value ?? string.Empty;
                OnPropertyChanged(nameof(Invoer));

                AantalKarakters = _invoer.Length;

                UpdateFormattedRegels();
                UpdateHintAndImage(); // ✅ hint altijd up-to-date
            }
        }

        private FormattedString _formattedVoorbeeldTekst;
        public FormattedString FormattedVoorbeeldTekst
        {
            get => _formattedVoorbeeldTekst;
            private set { _formattedVoorbeeldTekst = value; OnPropertyChanged(nameof(FormattedVoorbeeldTekst)); }
        }

        private FormattedString _formattedInvoer;
        public FormattedString FormattedInvoer
        {
            get => _formattedInvoer;
            private set { _formattedInvoer = value; OnPropertyChanged(nameof(FormattedInvoer)); }
        }

        private string _tijd = "00:00";
        public string Tijd
        {
            get => _tijd;
            private set { _tijd = value; OnPropertyChanged(nameof(Tijd)); }
        }

        private bool _startEnabled = true;
        public bool StartEnabled
        {
            get => _startEnabled;
            private set { _startEnabled = value; OnPropertyChanged(nameof(StartEnabled)); }
        }

        private bool _stopEnabled;
        public bool StopEnabled
        {
            get => _stopEnabled;
            private set { _stopEnabled = value; OnPropertyChanged(nameof(StopEnabled)); }
        }

        private bool _resultaatVisible;
        public bool ResultaatVisible
        {
            get => _resultaatVisible;
            private set { _resultaatVisible = value; OnPropertyChanged(nameof(ResultaatVisible)); }
        }

        private string _resultaatTekst = "";
        public string ResultaatTekst
        {
            get => _resultaatTekst;
            private set { _resultaatTekst = value; OnPropertyChanged(nameof(ResultaatTekst)); }
        }

        public bool Started { get; private set; }

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
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            if (level == null) throw new ArgumentNullException(nameof(level));

            VoorbeeldTekst = level.Tekst ?? "";
            SpelerId = 1;
            LevelId = level.Id;
            IsOefening = is_oefening;

            Tijd = "00:00";
            AantalKarakters = 0;

            StartEnabled = true;
            StopEnabled = false;
            ResultaatVisible = false;
            Started = false;

            BereidRegelsVoor();
            Invoer = string.Empty;

            // ✅ hint direct zichtbaar zodra je in de oefening komt
            UpdateFormattedRegels();
            UpdateHintAndImage();
        }

        #region Regels + Rendering
        private void BereidRegelsVoor()
        {
            _regels.Clear();
            int index = 0;

            while (index < VoorbeeldTekst.Length)
            {
                int resterend = VoorbeeldTekst.Length - index;

                if (resterend <= MinKaraktersLaatsteRegel && _regels.Count > 0)
                {
                    _regels[^1] += VoorbeeldTekst.Substring(index);
                    break;
                }

                int maxEinde = Math.Min(index + MaxKaraktersPerRegel, VoorbeeldTekst.Length);
                int split = maxEinde;

                while (split > index && VoorbeeldTekst[split - 1] != ' ')
                    split--;

                if (split == index)
                    split = maxEinde;

                _regels.Add(VoorbeeldTekst.Substring(index, split - index));
                index = split;
            }

            _huidigeRegelIndex = 0;
        }

        private int StartIndexVanRegel(int regel)
        {
            int idx = 0;
            for (int i = 0; i < regel; i++)
                idx += _regels[i].Length;
            return idx;
        }

        private void UpdateFormattedRegels()
        {
            var voorbeeld = new FormattedString();
            var invoer = new FormattedString();

            if (_regels.Count == 0)
            {
                FormattedVoorbeeldTekst = new FormattedString();
                FormattedInvoer = new FormattedString();
                return;
            }

            int eerste = Math.Max(0, _huidigeRegelIndex - 1);
            int laatste = Math.Min(_regels.Count - 1, _huidigeRegelIndex + 1);

            for (int r = eerste; r <= laatste; r++)
            {
                string regel = _regels[r];
                int start = StartIndexVanRegel(r);

                for (int i = 0; i < regel.Length; i++)
                {
                    int index = start + i;

                    Color kleur = index < Invoer.Length
                        ? index == _firstErrorIndex ? Colors.Red : Colors.Gray
                        : Colors.Black;

                    voorbeeld.Spans.Add(new Span { Text = regel[i].ToString(), TextColor = kleur });

                    if (r == _huidigeRegelIndex && index < Invoer.Length)
                    {
                        char c = Invoer[index];
                        invoer.Spans.Add(new Span
                        {
                            Text = c == ' ' ? "_" : c.ToString(),
                            TextColor = index == _firstErrorIndex ? Colors.Red : Colors.Black
                        });
                    }
                }

                voorbeeld.Spans.Add(new Span { Text = "\n" });
                if (r == _huidigeRegelIndex)
                    invoer.Spans.Add(new Span { Text = "\n" });
            }

            FormattedVoorbeeldTekst = voorbeeld;
            FormattedInvoer = invoer;
        }
        #endregion

        #region Typen
        public void VoegKarakterToe(char c)
        {
            if (_firstErrorIndex != -1) return;
            if (Invoer.Length >= VoorbeeldTekst.Length) return;

            if (c != VoorbeeldTekst[Invoer.Length])
            {
                _firstErrorIndex = Invoer.Length;
                _fouten.Add((Invoer.Length, c, VoorbeeldTekst[Invoer.Length]));
            }

            Invoer += c;

            if (_regels.Count > 0)
            {
                int einde = StartIndexVanRegel(_huidigeRegelIndex) + _regels[_huidigeRegelIndex].Length;
                if (Invoer.Length > einde)
                    _huidigeRegelIndex = Math.Min(_huidigeRegelIndex + 1, _regels.Count - 1);
            }

            if (Invoer.Length == VoorbeeldTekst.Length)
                StopOefening();
        }

        public void VerwijderLaatste()
        {
            if (Invoer.Length == 0) return;

            Invoer = Invoer[..^1];
            if (_firstErrorIndex >= Invoer.Length)
                _firstErrorIndex = -1;
        }
        #endregion

        #region Controls
        public void Start()
        {
            if (Started) return;

            Started = true;
            StartEnabled = false;
            StopEnabled = true;
            ResultaatVisible = false;

            _timerLoopt = true;
            _stopwatch.Restart();

            _dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (!_timerLoopt) return false;
                Tijd = _stopwatch.Elapsed.ToString(@"mm\:ss");
                return true;
            });

            // ✅ zet hint nog eens bij start
            UpdateHintAndImage();
        }

        public void Stop() => StopOefening();

        public void Opnieuw()
        {
            _stopwatch.Reset();
            Tijd = "00:00";

            Invoer = string.Empty;
            _fouten.Clear();
            _firstErrorIndex = -1;
            _huidigeRegelIndex = 0;

            Started = false;
            _timerLoopt = false;
            StartEnabled = true;
            StopEnabled = false;
            ResultaatVisible = false;

            UpdateFormattedRegels();
            UpdateHintAndImage();
        }

        private void StopOefening()
        {
            if (!_timerLoopt) return;

            _timerLoopt = false;
            _stopwatch.Stop();

            StartEnabled = true;
            StopEnabled = false;

            ToonResultaat();
        }
        #endregion

        #region Resultaat
        private void ToonResultaat()
        {
            var tijd = _stopwatch.Elapsed;

            int totalTyped = Invoer?.Length ?? 0;
            int foutenAantal = _fouten.Count;

            double accuracy = totalTyped > 0
                ? ((totalTyped - foutenAantal) / (double)totalTyped) * 100.0
                : 0.0;

            int woorden = Invoer.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            var wpm = tijd.TotalMinutes > 0 ? Math.Round(woorden / tijd.TotalMinutes, 2) : 0;

            ResultaatTekst =
                $"Tijd: {tijd:mm\\:ss}\n" +
                $"Karakters: {AantalKarakters}\n" +
                $"Woorden: {woorden}\n" +
                $"WPM: {wpm}\n" +
                $"Accuracy: {Math.Round(accuracy, 2)}%\n" +
                $"Fouten: {foutenAantal}";

            bool volledigGetypt = totalTyped == VoorbeeldTekst.Length;
            bool accuracyOk = accuracy >= 90.0;
            bool wpmOk = wpm >= 20;

            bool levelGehaald = volledigGetypt && accuracyOk && wpmOk;

            if (levelGehaald)
            {
                PracticeSession.MarkLevelGehaald(LevelId);

                if (!IsOefening)
                    _leaderboardService.plaats_score(LevelId, SpelerId, 900);

                ResultaatTekst += "\n\nLevel gehaald!";
            }
            else
            {
                ResultaatTekst += "\n\nLevel NIET gehaald.";
            }

            ResultaatVisible = true;

            // einde -> hints uit
            HuidigeHint = "";
            CurrentLetterImage = null;
        }
        #endregion

        #region Hint + Image (ALTIJD AAN)
        private void UpdateHintAndImage()
        {
            if (string.IsNullOrEmpty(VoorbeeldTekst) || Invoer.Length >= VoorbeeldTekst.Length)
            {
                HuidigeHint = "";
                CurrentLetterImage = null;
                return;
            }

            char volgende = VoorbeeldTekst[Invoer.Length];

            HuidigeHint = _vingerHints.TryGetValue(volgende, out var hint)
                ? hint
                : $"Volgende toets: '{volgende}'";

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
                CurrentLetterImage = "puntkomma.png";
                return;
            }

            CurrentLetterImage = null;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string naam) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));
    }
}
