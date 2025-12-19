using BasisJaar2.Models;
using BasisJaar2.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace BasisJaar2.Views
{
    public partial class Oefening : ContentView
    {
        private readonly ContentView _previousPage;

        // ✅ NIEUW: practiceModeHints expliciet meegeven (true/false)
        public Oefening(Level level, ContentView previousPage = null, bool practiceModeHints = false)
        {
            InitializeComponent();

            _previousPage = previousPage;

            var vm = new OefeningViewModel(this.Dispatcher, level);

 

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
            // refresh levels icons/knoppen als je teruggaat
            if (_previousPage is LevelsPage levelsPage &&
                levelsPage.BindingContext is LevelsViewModel vm)
            {
                vm.RefreshLevelStates();
            }

            if (_previousPage != null && MainPageViewModel.Current != null)
                MainPageViewModel.Current.SubpageContent = _previousPage;
            else if (MainPageViewModel.Current != null)
                MainPageViewModel.Current.SubpageContent = new MoeilijkheidsgraadPage();
        }
    }
}
