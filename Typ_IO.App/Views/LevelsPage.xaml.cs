using System;
using Microsoft.Maui.Controls;
using BasisJaar2.Models;

namespace BasisJaar2.Views
{
    public partial class LevelsPage : ContentView
    {
        public LevelsPage()
        {
            InitializeComponent();
        }

        private async void OnStartLevelClicked(object sender, EventArgs e)
        {
            if (sender is not Button button || button.BindingContext is not Level level)
                return;

            // 🔒 Check of het level wel unlocked is
            if (!PracticeSession.IsLevelUnlocked(level.Nummer))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Level vergrendeld",
                    $"Je moet eerst level {level.Nummer - 1} halen voordat je dit level kunt spelen.",
                    "OK");
                return;
            }

            // gekozen level opslaan
            PracticeSession.GeselecteerdLevel = level;

            // naar PracticePage
            if (ViewModels.MainPageViewModel.Current != null)
            {
                ViewModels.MainPageViewModel.Current.SubpageContent =
                    new PracticePage();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "MainPageViewModel not found",
                    "OK");
            }
        }
    }
}
