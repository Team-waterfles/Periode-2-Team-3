using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace BasisJaar2.ViewModels
{
    public class MoeilijkheidsgraadViewModel : BindableObject
    {
        // commands voor de knoppen
        public ICommand MakkelijkCommand { get; }
        public ICommand NormaalCommand { get; }
        public ICommand MoeilijkCommand { get; }
        public ICommand ExitCommand { get; }

        // constructor - hier maken we de commands aan
        public MoeilijkheidsgraadViewModel()
        {
            MakkelijkCommand = new Command(OnMakkelijk);
            NormaalCommand = new Command(OnNormaal);
            MoeilijkCommand = new Command(OnMoeilijk);
            ExitCommand = new Command(OnExit);
        }

        // deze functie wordt aangeroepen als je op Makkelijk klikt
        private void OnMakkelijk()
        {
            // hier kan je later code toevoegen voor het makkelijke niveau
            if (Application.Current?.MainPage != null)
            {
                Application.Current.MainPage.DisplayAlert("Moeilijkheidsgraad", "Je hebt Makkelijk gekozen!", "OK");
            }
        }

        // deze functie wordt aangeroepen als je op Normaal klikt
        private void OnNormaal()
        {
            // hier kan je later code toevoegen voor het normale niveau
            if (Application.Current?.MainPage != null)
            {
                Application.Current.MainPage.DisplayAlert("Moeilijkheidsgraad", "Je hebt Normaal gekozen!", "OK");
            }
        }

        // deze functie wordt aangeroepen als je op Moeilijk klikt
        private void OnMoeilijk()
        {
            // hier kan je later code toevoegen voor het moeilijke niveau
            if (Application.Current?.MainPage != null)
            {
                Application.Current.MainPage.DisplayAlert("Moeilijkheidsgraad", "Je hebt Moeilijk gekozen!", "OK");
            }
        }

        // deze functie wordt aangeroepen als je op Exit klikt
        private void OnExit()
        {
            // ga terug naar het startscherm
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new Views.StartScreen();
            }
        }
    }
}