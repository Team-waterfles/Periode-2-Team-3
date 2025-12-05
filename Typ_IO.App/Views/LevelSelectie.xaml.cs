using BasisJaar2.ViewModels;

namespace BasisJaar2.Views;

public partial class LevelSelectie : ContentView
{
    public string Difficulty { get; }

    public LevelSelectie(string difficulty)
    {
        InitializeComponent();

        Difficulty = difficulty?.ToLower() ?? "makkelijk";

        BindingContext = new LevelSelectieViewModel(Difficulty);
    }

    private void OnTerugClicked(object sender, EventArgs e)
    {
        if (MainPageViewModel.Current != null)
        {
            var currentPage = MainPageViewModel.Current.SubpageContent;

            MainPageViewModel.Current.SubpageContent =
                new MoeilijkheidsgraadPage();
        }
    }
}
