using BasisJaar2.ViewModels;
using Typ_IO.Core.Models;

namespace BasisJaar2.Views;

public partial class Levelleaderboard : ContentView
{
	public Levelleaderboard(Standaardlevel level)
	{
		InitializeComponent();
        BindingContext = new LevelleaderboardViewModel(level);
    }
}