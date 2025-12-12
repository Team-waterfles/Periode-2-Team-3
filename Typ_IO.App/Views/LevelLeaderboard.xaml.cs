using BasisJaar2.ViewModels;

namespace BasisJaar2.Views;

public partial class LevelLeaderboard : ContentView
{
	public LevelLeaderboard()
	{
		InitializeComponent();
        BindingContext = new LevelLeaderboardViewModel();
    }
}