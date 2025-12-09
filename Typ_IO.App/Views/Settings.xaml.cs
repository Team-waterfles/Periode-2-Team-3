namespace BasisJaar2.Views;

public partial class Settings : ContentView
{

    public Settings()
    {
        InitializeComponent();
        BindingContext = new ViewModels.SettingsViewModel();
    }
}