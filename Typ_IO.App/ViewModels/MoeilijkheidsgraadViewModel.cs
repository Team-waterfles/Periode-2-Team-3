using Microsoft.Maui.Controls;
using System.Windows.Input;
using BasisJaar2.Views;

namespace BasisJaar2.ViewModels
{
    public class MoeilijkheidsgraadViewModel : BindableObject
    {
        // Commands voor de knoppen
        public ICommand MakkelijkCommand { get; }
        public ICommand NormaalCommand { get; }
        public ICommand MoeilijkCommand { get; }
        public ICommand ExitCommand { get; }

        // Constructor
        public MoeilijkheidsgraadViewModel()
        {
            MakkelijkCommand = new Command(OnMakkelijk);
            NormaalCommand = new Command(OnNormaal);
            MoeilijkCommand = new Command(OnMoeilijk);
            ExitCommand = new Command(OnExit);
        }

        // Functies voor de buttons
        private void OnMakkelijk()
        {
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new LevelSelectie("makkelijk");
            }
        }

        private void OnNormaal()
        {
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new LevelSelectie("gemiddeld");
            }
        }

        private void OnMoeilijk()
        {
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new LevelSelectie("moeilijk");
            }
        }

        private void OnExit()
        {
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new Views.StartScreen();
            }
        }
    }
}
