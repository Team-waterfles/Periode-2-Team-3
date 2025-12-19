using BasisJaar2.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasisJaar2.Views
{
    public partial class Oefening : ContentView
    {
        private readonly string _levelKey;
        private readonly ContentView _previousPage;

        public static readonly Dictionary<string, string> Voorbeelden = new()
        {
            { "Easy", "de snelle bruine vos springt over de luie hond." },
            { "Medium", "typ deze zin zorgvuldig om je snelheid te testen." },
            { "Hard", "dit is een moeilijkere oefening voor gevorderde typers." },
            { "1", "fj fjfjf jfff jfjf fjf fjjj ffjf fjfj fjjfff jffj jfj jfjjj fjfffj fffj jfjj" },
            { "2", "fjdj kfj djek fjek idid kejf fjdkk jfddf kjdfk fjkjd dffjk dkjff jfkdj fdfk dfkjf"},
            { "3", "wslo slwo fdsj slwo wofs dslj wslo slwo fdsj slwo wofs dslj wofs slwj fdsj wlsj slwo" },
            { "4", "asdf jkl; asdf jkl; sadf lask fjda ;lkj asdf jkl; sadf lask fjda ;lkj asdf jkla sdfj lkask" },
            { "5", "qwer tyui opqw type type quit quit write type qwe rtyui opqwe rtyuio pqw ertyuiop qwert yuiopq" },
            { "6", "asdf jkl; qwer tyui zxcv bnm type snel met alle vingers qaz wsx edc rfvtgb yhnujm kiolp azwsx edcrf" },
            { "level1", "asdf jkl asdf jkl fff jjj aaa sss ddd kkk lll asdf jkl asdf jkl fff jjj aaa sss ddd kkk lll asdf jkl asdf jkl fff jjj aaa sss ddd kkk lll;" },
            { "level2", "qwer asdf zxcv uiop jkl qwer asdf zxcv typen leren is leuk en nuttig voor iedereen qwer asdf zxcv uiop jkl typen leren oefening"},
            { "level3", "hallo wereld auto fiets computer toetsenbord oefen typen leren school student programmeren muziek ritme spelen piano gitaar drums zingen Nederland Amsterdam Rotterdam Utrecht Groningen"},
            { "level4", "De kat zit op de mat. De hond rent door het park. Typen leren is leuk en nuttig voor iedereen. Muziek en ritme helpen bij het oefenen. Practice makes perfect, dus blijf oefenen!"},
            { "level5", "Dit is een uitgebreide typoefening voor gevorderde gebruikers. Typen is een belangrijke vaardigheid in de moderne wereld. Of je nu student bent, professional of hobbyist, goede typvaardigheden maken je werk een stuk efficiënter. Door regelmatig te oefenen met verschillende soorten teksten, verbeter je niet alleen je snelheid maar ook je nauwkeurigheid. Muziek en ritme kunnen helpen om je typritme te verbeteren en het oefenen aangenamer te maken."}
        };

        public Oefening(string key, string voorbeeldTekstFromSuggested = null, ContentView previousPage = null)
        {
            InitializeComponent();

            _previousPage = previousPage;
            _levelKey = key ?? string.Empty;

            string voorbeeldTekst;

            if (!string.IsNullOrEmpty(voorbeeldTekstFromSuggested))
            {
                // suggested level: gebruik de meegegeven tekst
                voorbeeldTekst = voorbeeldTekstFromSuggested;
            }
            else
            {
                // normaal level: kies voorbeeldtekst
                voorbeeldTekst = Voorbeelden.ContainsKey(key)
                    ? Voorbeelden[key]
                    : Voorbeelden["Easy"];
            }

            var vm = new OefeningViewModel(this.Dispatcher, voorbeeldTekst, _levelKey);

            if (_previousPage is LevelsPage)
                vm.PracticeModeHints = true;

            BindingContext = vm;

            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(50);
                HiddenEditor?.Focus();
            });
        }

        private OefeningViewModel VM => BindingContext as OefeningViewModel;

        private void HiddenEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (VM == null) return;

            string nieuweTekst = e.NewTextValue ?? string.Empty;

            if (!VM.Started && nieuweTekst.Length > 0)
                VM.Start();

            if (nieuweTekst.Length < VM.Invoer.Length)
                VM.VerwijderLaatste();
            else if (nieuweTekst.Length > VM.Invoer.Length)
                VM.VoegKarakterToe(nieuweTekst[^1]);

            ((Editor)sender).Text = VM.Invoer;
        }

        private void Start_Clicked(object sender, EventArgs e) => VM?.Start();
        private void Stop_Clicked(object sender, EventArgs e) => VM?.Stop();
        private void Opnieuw_Clicked(object sender, EventArgs e) => VM?.Opnieuw();

        private void Terug_Clicked(object sender, EventArgs e)
        {
            if (_previousPage != null && MainPageViewModel.Current != null)
                MainPageViewModel.Current.SubpageContent = _previousPage;
            else if (MainPageViewModel.Current != null)
                MainPageViewModel.Current.SubpageContent = new MoeilijkheidsgraadPage();
        }
    }
}