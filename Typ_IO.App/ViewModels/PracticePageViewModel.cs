using Microsoft.Maui.Controls;
using System;
using System.Windows.Input;
using BasisJaar2.Models;

namespace BasisJaar2.ViewModels
{
    public class PracticePageViewModel : BindableObject
    {
        private string _levelNaam = "";
        public string LevelNaam
        {
            get => _levelNaam;
            set { _levelNaam = value; OnPropertyChanged(); }
        }

        private string _oefentekst = "";
        public string Oefentekst
        {
            get => _oefentekst;
            set { _oefentekst = value; OnPropertyChanged(); }
        }

        private string _userText = "";
        public string UserText
        {
            get => _userText;
            set { _userText = value; OnPropertyChanged(); }
        }

        private string _resultaatTekst = "";
        public string ResultaatTekst
        {
            get => _resultaatTekst;
            set { _resultaatTekst = value; OnPropertyChanged(); }
        }

        public ICommand StartCommand { get; }
        public ICommand KlaarCommand { get; }

        private bool _gestart;
        private DateTime _startTijd;

        public PracticePageViewModel()
        {
            var level = PracticeSession.GeselecteerdLevel;
            if (level != null)
            {
                LevelNaam = level.Naam;
                Oefentekst = level.Oefentekst;
            }
            else
            {
                LevelNaam = "Geen level gekozen";
                Oefentekst = "Ga terug en kies eerst een level.";
            }

            UserText = string.Empty;
            ResultaatTekst = string.Empty;

            StartCommand = new Command(OnStart);
            KlaarCommand = new Command(OnKlaar);
        }

        private void OnStart()
        {
            _gestart = true;
            _startTijd = DateTime.Now;
            UserText = string.Empty;
            ResultaatTekst = string.Empty;
            OnPropertyChanged(nameof(UserText));
            OnPropertyChanged(nameof(ResultaatTekst));
        }

        private async void OnKlaar()
        {
            if (!_gestart)
                return;

            _gestart = false;
            var duur = DateTime.Now - _startTijd;

            // fouten berekenen
            BerekenFouten(out int fouten, out double nauwkeurigheid);

            // tekst voor onderin het scherm
            ResultaatTekst = $"Fouten: {fouten}  •  Nauwkeurigheid: {nauwkeurigheid:P0}";
            OnPropertyChanged(nameof(ResultaatTekst));

            // popup met tijd + fouten
            await Application.Current.MainPage.DisplayAlert(
                "Oefening afgerond",
                $"Tijd: {duur.TotalSeconds:F1} sec\nFouten: {fouten}\nNauwkeurigheid: {nauwkeurigheid:P0}",
                "OK");
        }

        private void BerekenFouten(out int fouten, out double nauwkeurigheid)
        {
            string referentie = Oefentekst ?? string.Empty;
            string input = UserText ?? string.Empty;

            int maxLen = Math.Max(referentie.Length, input.Length);
            int correct = 0;
            int fout = 0;

            for (int i = 0; i < maxLen; i++)
            {
                char refChar = i < referentie.Length ? referentie[i] : '\0';
                char inpChar = i < input.Length ? input[i] : '\0';

                if (refChar == '\0' && inpChar == '\0')
                {
                    // niks meer te vergelijken
                    break;
                }

                if (refChar == inpChar && refChar != '\0')
                    correct++;
                else
                    fout++;
            }

            fouten = fout;
            int totaal = correct + fout;
            nauwkeurigheid = totaal > 0 ? (double)correct / totaal : 0;
        }
    }
}
