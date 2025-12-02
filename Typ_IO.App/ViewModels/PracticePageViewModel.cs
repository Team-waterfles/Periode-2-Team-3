using Microsoft.Maui.Controls;
using System;
using System.Windows.Input;
using BasisJaar2.Models;

namespace BasisJaar2.ViewModels
{
    public class PracticePageViewModel : BindableObject
    {
        public string LevelNaam { get; set; }
        public string Oefentekst { get; set; }

        private string _userText = "";
        public string UserText
        {
            get => _userText;
            set
            {
                if (_userText == value) return;
                _userText = value;
                OnPropertyChanged();

                // timer starten bij eerste toetsaanslag
                if (_gestart && !_timerGestart && !string.IsNullOrEmpty(_userText))
                {
                    _timerGestart = true;
                    _startTijd = DateTime.Now;
                }

                // timer stoppen als lengte oefentekst is bereikt of overschreden
                if (_gestart && _timerGestart && !_timerGestopt &&
                    _userText.Length >= Oefentekst.Length)
                {
                    _timerGestopt = true;
                    _eindTijd = DateTime.Now;
                }

                // keyboard assist bijwerken
                UpdateKeyboardAssist();
            }
        }

        private string _tijdTekst = "";
        public string TijdTekst
        {
            get => _tijdTekst;
            set { _tijdTekst = value; OnPropertyChanged(); }
        }

        private string _resultaatTekst = "";
        public string ResultaatTekst
        {
            get => _resultaatTekst;
            set { _resultaatTekst = value; OnPropertyChanged(); }
        }

        private string _wpmTekst = "";
        public string WpmTekst
        {
            get => _wpmTekst;
            set { _wpmTekst = value; OnPropertyChanged(); }
        }

        public bool KanStarten { get; set; } = true;
        public bool KanKlaar { get; set; } = false;

        private bool _kanTypen = false;
        public bool KanTypen
        {
            get => _kanTypen;
            set { _kanTypen = value; OnPropertyChanged(); }
        }

        // 👇 Keyboard assist properties
        private string _huidigeToets = "";
        public string HuidigeToets
        {
            get => _huidigeToets;
            set { _huidigeToets = value; OnPropertyChanged(); }
        }

        private string _huidigeVinger = "";
        public string HuidigeVinger
        {
            get => _huidigeVinger;
            set { _huidigeVinger = value; OnPropertyChanged(); }
        }

        public ICommand StartCommand { get; }
        public ICommand KlaarCommand { get; }

        private bool _gestart;
        private bool _timerGestart;
        private bool _timerGestopt;
        private DateTime _startTijd;
        private DateTime _eindTijd;

        public PracticePageViewModel()
        {
            var level = PracticeSession.GeselecteerdLevel;
            LevelNaam = level?.Naam ?? "Geen level gekozen";
            Oefentekst = level?.Oefentekst ?? "";

            StartCommand = new Command(OnStart);
            KlaarCommand = new Command(OnKlaar);

            KanTypen = false;

            // eerste toets-hint tonen
            UpdateKeyboardAssist();
        }

        private void OnStart()
        {
            _gestart = true;
            _timerGestart = false;
            _timerGestopt = false;
            _startTijd = DateTime.MinValue;
            _eindTijd = DateTime.MinValue;

            UserText = string.Empty;
            TijdTekst = "";
            WpmTekst = "";
            ResultaatTekst = "";

            KanStarten = false;
            KanKlaar = true;
            KanTypen = true;

            OnPropertyChanged(nameof(KanStarten));
            OnPropertyChanged(nameof(KanKlaar));
            OnPropertyChanged(nameof(KanTypen));

            UpdateKeyboardAssist();
        }

        private async void OnKlaar()
        {
            if (!_gestart)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Let op",
                    "Druk eerst op Start voordat je klaar bent.",
                    "OK");
                return;
            }

            if (_timerGestart && !_timerGestopt)
            {
                _eindTijd = DateTime.Now;
                _timerGestopt = true;
            }

            _gestart = false;
            KanStarten = true;
            KanKlaar = false;
            KanTypen = false;

            OnPropertyChanged(nameof(KanStarten));
            OnPropertyChanged(nameof(KanKlaar));
            OnPropertyChanged(nameof(KanTypen));

            var duur = (_timerGestart && _eindTijd != DateTime.MinValue)
                ? _eindTijd - _startTijd
                : TimeSpan.Zero;

            BerekenFouten(out int fouten, out double nauwkeurigheid);

            int woorden = TelWoorden(UserText);
            double minuten = duur.TotalMinutes;
            double wpm = (minuten > 0) ? woorden / minuten : 0;

            TijdTekst = $"Tijd: {duur.TotalSeconds:F1} sec";
            WpmTekst = $"Snelheid: {wpm:F0} WPM";
            ResultaatTekst = $"Fouten: {fouten} • Nauwkeurigheid: {nauwkeurigheid:P0}";

            // 🔓 LEVEL UNLOCK LOGICA
            bool levelGehaald = false;
            const double minimaleNauwkeurigheid = 0.90; // 90%
            const double minimaleWpm = 20;              // 20 WPM

            if (nauwkeurigheid >= minimaleNauwkeurigheid && wpm >= minimaleWpm)
            {
                levelGehaald = true;
            }

            var huidigLevel = PracticeSession.GeselecteerdLevel;
            if (huidigLevel != null && levelGehaald)
            {
                PracticeSession.MarkLevelGehaald(huidigLevel.Nummer);
                ResultaatTekst += " • Level gehaald!";
            }
            else
            {
                ResultaatTekst += " • Level NIET gehaald.";
            }

            await Application.Current.MainPage.DisplayAlert(
                "Resultaat",
                $"{TijdTekst}\n{WpmTekst}\n{ResultaatTekst}",
                "OK");

            // na klaar: ook keyboard assist bijwerken (bijv. ✓)
            UpdateKeyboardAssist();
        }

        private void UpdateKeyboardAssist()
        {
            if (string.IsNullOrEmpty(Oefentekst))
            {
                HuidigeToets = "";
                HuidigeVinger = "";
                return;
            }

            int index = UserText?.Length ?? 0;

            if (index >= Oefentekst.Length)
            {
                HuidigeToets = "✓";
                HuidigeVinger = "Tekst klaar";
                return;
            }

            char c = Oefentekst[index];
            HuidigeToets = c.ToString();

            HuidigeVinger = c switch
            {
                'f' or 'F' => "Linker wijsvinger (F)",
                'j' or 'J' => "Rechter wijsvinger (J)",
                'd' or 'D' => "Linker middelvinger (D)",
                'k' or 'K' => "Rechter middelvinger (K)",
                's' or 'S' => "Linker ringvinger (S)",
                'l' or 'L' => "Rechter ringvinger (L)",
                'a' or 'A' => "Linker pink (A)",
                ';' => "Rechter pink (;)",
                'g' or 'G' => "Linker wijsvinger (G)",
                'h' or 'H' => "Rechter wijsvinger (H)",
                't' or 'T' => "Linker wijsvinger (T)",
                'y' or 'Y' => "Rechter wijsvinger (Y)",
                'r' or 'R' => "Linker wijsvinger (R)",
                'u' or 'U' => "Rechter wijsvinger (U)",
                'e' or 'E' => "Linker middelvinger (E)",
                'i' or 'I' => "Rechter middelvinger (I)",
                'w' or 'W' => "Linker ringvinger (W)",
                'o' or 'O' => "Rechter ringvinger (O)",
                'q' or 'Q' => "Linker pink (Q)",
                'p' or 'P' => "Rechter pink (P)",
                'z' or 'Z' => "Linker ringvinger (Z)",
                'x' or 'X' => "Linker ringvinger (X)",
                'c' or 'C' => "Linker middelvinger (C)",
                'v' or 'V' => "Linker wijsvinger (V)",
                'b' or 'B' => "Linker wijsvinger / duim",
                'n' or 'N' => "Rechter wijsvinger (N)",
                'm' or 'M' => "Rechter middelvinger (M)",
                ' ' => "Spatiebalk (duim)",
                _ => "Gebruik de juiste vinger voor deze toets"
            };
        }

        private void BerekenFouten(out int fouten, out double nauwkeurigheid)
        {
            fouten = 0;
            int correct = 0;

            for (int i = 0; i < Oefentekst.Length; i++)
            {
                if (i < UserText.Length && UserText[i] == Oefentekst[i])
                    correct++;
                else
                    fouten++;
            }

            nauwkeurigheid = Oefentekst.Length > 0
                ? (double)correct / Oefentekst.Length
                : 0;
        }

        private int TelWoorden(string tekst)
        {
            if (string.IsNullOrWhiteSpace(tekst))
                return 0;

            return tekst.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
