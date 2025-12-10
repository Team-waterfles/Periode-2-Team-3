using BasisJaar2.ViewModels;

namespace BasisJaar2.Views;

public partial class Settings : ContentView
{
    public Settings()
    {
        InitializeComponent();
        BindingContext = new ViewModels.SettingsViewModel();
    }

    private void OnTerugClicked(object sender, EventArgs e)
    {
        if (MainPageViewModel.Current != null)
        {
            MainPageViewModel.Current.SubpageContent = new StartScreen();
        }
    }
}