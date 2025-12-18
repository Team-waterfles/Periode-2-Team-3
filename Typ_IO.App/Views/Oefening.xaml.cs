using BasisJaar2.ViewModels;
using BasisJaar2.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Typ_IO.Core.Models;

namespace BasisJaar2.Views
{
    public partial class Oefening : ContentView
    {
        private readonly ContentView _previousPage;

        public Oefening(Level level, bool is_oefening, ContentView previousPage = null)
        {
            InitializeComponent();

            _previousPage = previousPage;

            string voorbeeldTekst = level.Tekst;

            var vm = new OefeningViewModel(this.Dispatcher, level, is_oefening);

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
