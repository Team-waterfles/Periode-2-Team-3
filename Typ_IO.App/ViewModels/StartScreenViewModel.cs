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
            // TODO: Add your play logic here
            Application.Current.MainPage.DisplayAlert("Play", "Play button clicked!", "OK");
        }

        private void OnExit()
        {
            // Exit the application
            System.Environment.Exit(0);
        }
    }
}
