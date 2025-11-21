using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Maui.Dispatching;

namespace BasisJaar2.ViewModels
{
    public class OefeningViewModel : INotifyPropertyChanged
    {
        private readonly IDispatcher _dispatcher;

        private Stopwatch _stopwatch;
        private bool _timerLoopt;

        public string VoorbeeldTekst { get; private set; }

        //-----------------------------------------------------
        // Constructor
        //-----------------------------------------------------
        public OefeningViewModel(IDispatcher dispatcher, string voorbeeldTekst)
        {
            _dispatcher = dispatcher;
            VoorbeeldTekst = voorbeeldTekst;

            _stopwatch = new Stopwatch();

            StartCommand = new Command(Start);
            StopCommand = new Command(Stop);
            OpnieuwCommand = new Command(Opnieuw);
            TerugCommand = new Command(Terug);
        }

        //-----------------------------------------------------
        // INotifyPropertyChanged
        //-----------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string naam)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));
        }

        //-----------------------------------------------------
        // Properties
        //-----------------------------------------------------
        private string _tijd = "00:00";
        public string Tijd
        {
            get => _tijd;
            set { _tijd = value; OnPropertyChanged(nameof(Tijd)); }
        }

        private string _invoer = string.Empty;
        public string Invoer
        {
            get => _invoer;
            set
            {
                _invoer = value;
                OnPropertyChanged(nameof(Invoer));

                AantalKarakters = _invoer?.Length ?? 0;

                if (_invoer.Length >= VoorbeeldTekst.Length)
                {
                    StopOefening();
                }
            }
        }

        private int _aantalKarakters;
        public int AantalKarakters
        {
            get => _aantalKarakters;
            set { _aantalKarakters = value; OnPropertyChanged(nameof(AantalKarakters)); }
        }

        private bool _invoerEnabled;
        public bool InvoerEnabled
        {
            get => _invoerEnabled;
            set { _invoerEnabled = value; OnPropertyChanged(nameof(InvoerEnabled)); }
        }

        private bool _startEnabled = true;
        public bool StartEnabled
        {
            get => _startEnabled;
            set { _startEnabled = value; OnPropertyChanged(nameof(StartEnabled)); }
        }

        private bool _stopEnabled = false;
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

        //-----------------------------------------------------
        // Commands
        //-----------------------------------------------------
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand OpnieuwCommand { get; }
        public ICommand TerugCommand { get; }

        //-----------------------------------------------------
        // Methoden
        //-----------------------------------------------------
        private void Start()
        {
            _stopwatch.Start();
            _timerLoopt = true;

            InvoerEnabled = true;
            Invoer = string.Empty;

            StartEnabled = false;
            StopEnabled = true;
            ResultaatVisible = false;

            StartTimerUpdate();
        }

        private void Stop()
        {
            StopOefening();
        }

        private void Terug()
        {
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new Views.MoeilijkheidsgraadPage();
            }
        }

        private void Opnieuw()
        {
            _stopwatch.Reset();
            Tijd = "00:00";
            AantalKarakters = 0;
            Invoer = "";
            InvoerEnabled = false;

            StartEnabled = true;
            StopEnabled = false;
            ResultaatVisible = false;
            _timerLoopt = false;
        }

        private void StopOefening()
        {
            _stopwatch.Stop();
            _timerLoopt = false;

            InvoerEnabled = false;

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

            ResultaatTekst =
                $"Tijd: {tijd:mm\\:ss}\n" +
                $"Karakters: {AantalKarakters}\n" +
                $"Woorden: {aantalWoorden}\n" +
                $"WPM: {wpm}";

            ResultaatVisible = true;
        }

        private int TelWoorden(string tekst)
        {
            if (string.IsNullOrWhiteSpace(tekst))
                return 0;

            return tekst.Split(new[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries).Length;
        }

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
