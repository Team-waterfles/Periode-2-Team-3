using Microsoft.Maui.Controls;
using System;
using System.Windows.Input;

namespace BasisJaar2.ViewModels
{
    public class StartScreenViewModel : BindableObject
    {
        public ICommand PlayCommand { get; }
        public ICommand PracticeCommand { get; }
        public ICommand ExitCommand { get; }

        public StartScreenViewModel()
        {
            PlayCommand = new Command(OnPlay);
            PracticeCommand = new Command(OnPractice);
            ExitCommand = new Command(OnExit);
        }

        private void OnPlay()
        {
            // Voor nu hetzelfde als Practice: ga naar LevelsPage
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent =
                    new BasisJaar2.Views.LevelsPage();
            }
            else
            {
                Application.Current?.MainPage?.DisplayAlert(
                    "Error",
                    "MainPageViewModel not found",
                    "OK");
            }
        }

        private void OnPractice()
        {
            // Practice mode -> ook naar LevelsPage
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent =
                    new BasisJaar2.Views.LevelsPage();
            }
            else
            {
                Application.Current?.MainPage?.DisplayAlert(
                    "Error",
                    "MainPageViewModel not found",
                    "OK");
            }
        }

        private void OnExit()
        {
            Environment.Exit(0);
        }
    }
}
