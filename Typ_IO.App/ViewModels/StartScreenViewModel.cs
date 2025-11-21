using Microsoft.Maui.Controls;
using System;
using System.Windows.Input;

namespace BasisJaar2.ViewModels
{
    public class StartScreenViewModel : BindableObject
    {
        public ICommand PlayCommand { get; }
        public ICommand ExitCommand { get; }

        public StartScreenViewModel()
        {
            PlayCommand = new Command(OnPlay);
            ExitCommand = new Command(OnExit);
        }

        private void OnPlay()
        {
            // gebruik de static Current property
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new Views.MoeilijkheidsgraadPage();
            }
            else
            {
                Application.Current?.MainPage?.DisplayAlert("Error", "MainPageViewModel not found", "OK");
            }
        }

        private void OnExit()
        {
            System.Environment.Exit(0);
        }
    }
}