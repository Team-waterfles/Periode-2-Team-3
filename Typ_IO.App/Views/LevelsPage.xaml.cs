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

        private void OnStartLevelClicked(object sender, EventArgs e)
        {
            // Haal het Level-object van deze button
            if (sender is Button button && button.BindingContext is Level level)
            {
                // Level doorgeven aan PracticePage
                PracticeSession.GeselecteerdLevel = level;
            }

            // Navigeren naar PracticePage
            if (ViewModels.MainPageViewModel.Current != null)
            {
                ViewModels.MainPageViewModel.Current.SubpageContent =
                    new PracticePage();
            }
            else
            {
                Application.Current?.MainPage?.DisplayAlert(
                    "Error",
                    "MainPageViewModel not found",
                    "OK");
            }
        }
    }
}
