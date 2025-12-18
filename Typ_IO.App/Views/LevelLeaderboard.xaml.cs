using BasisJaar2.ViewModels;
using Typ_IO.Core.Models;

namespace BasisJaar2.Views;

public partial class LevelLeaderboard : ContentView
{
	public LevelLeaderboard(Standaardlevel level)
	{
		InitializeComponent();
        BindingContext = new LevelLeaderboardViewModel(level);
    }
    private void OnTerugClicked(object sender, EventArgs e)
    {
        if (MainPageViewModel.Current != null)
        {
            MainPageViewModel.Current.SubpageContent =
                new MoeilijkheidsgraadPage();
        }
    }
}