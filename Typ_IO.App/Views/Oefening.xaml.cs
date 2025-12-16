using BasisJaar2.Models;
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

            // Levels 1 t/m 10
            { "1", "fj fjfjf jfff jfjf fjf fjjj ffjf fjfj fjjfff jffj jfj jfjjj fjfffj fffj jfjj" },
            { "2", "fjdj kfj djek fjek idid kejf fjdkk jfddf kjdfk fjkjd dffjk dkjff jfkdj fdfk dfkjf" },
            { "3", "wslo slwo fdsj slwo wofs dslj wslo slwo fdsj slwo wofs dslj wofs slwj fdsj wlsj slwo" },
            { "4", "asdf jkl; asdf jkl; sadf lask fjda ;lkj asdf jkl; sadf lask fjda ;lkj asdf jkla sdfj lkask" },
            { "5", "qwer tyui opqw type type quit quit write type qwe rtyui opqwe rtyuio pqw ertyuiop qwert yuiopq" },
            { "6", "asdf jkl; qwer tyui zxcv bnm type snel met alle vingers qaz wsx edc rfvtgb yhnujm kiolp azwsx edcrf" },

            { "7", "kat met pet, vis in glas, jas op kapstok, typ korte woorden snel achter elkaar: kat pet jas vis glas kast tas mat rat lat." },
            { "8", "vandaag gaan we rustig zinnen typen zonder hoofdletters of leestekens zodat je volledig op het ritme en de aanslagen kunt letten tijdens het oefenen met blind typen" },
            { "9", "Vandaag typen we zinnen met hoofdletters en komma's. Let goed op spaties, leestekens en ritme, zodat je nauwkeurig én snel leert typen, zonder te veel fouten te maken." },
            { "10", "Dit is een langere oefening met complete zinnen. Blind typen helpt je om sneller en efficiënter te werken op school, op je werk en thuis. Hoe vaker je oefent, hoe beter je spiergeheugen wordt. Blijf rustig ademen, kijk naar het scherm in plaats van naar het toetsenbord en probeer een gelijkmatig tempo aan te houden." }
        };

        // Dit is de "hoofd"-constructor die we gebruiken vanuit LevelsPage
        public Oefening(string key, string source = "play", ContentView previousPage = null)
        {
            InitializeComponent();

            _previousPage = previousPage;
            _levelKey = key ?? string.Empty;

            string voorbeeldTekst = Voorbeelden.ContainsKey(key)
                ? Voorbeelden[key]
                : Voorbeelden["1"];

            // LET OP: alleen 2 parameters, zodat hij matcht met jouw huidige OefeningViewModel
            var levelObj = new Level
            {
                Nummer = _levelKey,
                Id = int.TryParse(_levelKey, out var id) ? id : 0,
                Naam = PracticeSession.GeselecteerdLevel?.Naam ?? $"Level {_levelKey}",
                Tekst = voorbeeldTekst
            };

            var vm = new OefeningViewModel(this.Dispatcher, levelObj);

            if (_previousPage is LevelsPage)
                vm.PracticeModeHints = true;

            BindingContext = vm;

            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(50);
                HiddenEditor?.Focus();
            });
        }

        // Extra constructor zodat oude code new Oefening(level, currentPage) blijft werken
        public Oefening(Level level, ContentView previousPage = null)
            : this(level?.Nummer ?? "1", "play", previousPage)
        {
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
            // ? Refresh levels zodat icons/knoppen updaten
            if (_previousPage is LevelsPage levelsPage)
            {
                if (levelsPage.BindingContext is LevelsViewModel vm)
                    vm.RefreshLevelStates();
            }

            // ? Gewoon terug navigeren
            if (_previousPage != null && MainPageViewModel.Current != null)
                MainPageViewModel.Current.SubpageContent = _previousPage;
            else if (MainPageViewModel.Current != null)
                MainPageViewModel.Current.SubpageContent = new MoeilijkheidsgraadPage();
        }

    }
}
