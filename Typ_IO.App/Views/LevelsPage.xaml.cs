using Microsoft.Maui.Controls;
using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class LevelsPage : ContentView
    {
        public LevelsPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.LevelsViewModel();
        }

        private void OnTerugClicked(object sender, EventArgs e)
        {
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new StartScreen();
            }
        }
    }
}