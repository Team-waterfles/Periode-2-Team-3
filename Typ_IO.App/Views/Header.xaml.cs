using BasisJaar2.ViewModels;

namespace BasisJaar2.Views;

public partial class Header : ContentView
{
    public Header()
    {
        InitializeComponent();
    }

    private void OnHomeClicked(object sender, EventArgs e)
    {
        if (MainPageViewModel.Current != null)
        {
            MainPageViewModel.Current.SubpageContent = new StartScreen();
        }
    }
}