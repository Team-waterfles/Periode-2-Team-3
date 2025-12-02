using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace BasisJaar2.ViewModels
{
    public class MoeilijkheidsgraadViewModel : BindableObject
    {
        // Event om navigatie te triggeren
        public event EventHandler? NavigeerNaarLevelSelectie;
        public event EventHandler<string>? MoeilijkheidsgraadGekozen;

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
            // Geef door welke moeilijkheidsgraad gekozen is
            MoeilijkheidsgraadGekozen?.Invoke(this, "Makkelijk");
            // Navigeer naar level selectie
            NavigeerNaarLevelSelectie?.Invoke(this, EventArgs.Empty);
        }

        // deze functie wordt aangeroepen als je op Normaal klikt
        private void OnNormaal()
        {
            // Geef door welke moeilijkheidsgraad gekozen is
            MoeilijkheidsgraadGekozen?.Invoke(this, "Normaal");
            // Navigeer naar level selectie
            NavigeerNaarLevelSelectie?.Invoke(this, EventArgs.Empty);
        }

        // deze functie wordt aangeroepen als je op Moeilijk klikt
        private void OnMoeilijk()
        {
            // Geef door welke moeilijkheidsgraad gekozen is
            MoeilijkheidsgraadGekozen?.Invoke(this, "Moeilijk");
            // Navigeer naar level selectie
            NavigeerNaarLevelSelectie?.Invoke(this, EventArgs.Empty);
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