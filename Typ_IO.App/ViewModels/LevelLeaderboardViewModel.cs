using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BasisJaar2.Models;
using BasisJaar2.Views;
using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;

namespace BasisJaar2.ViewModels;
public partial class LevelLeaderboardViewModel : BindableObject
{
    public ObservableCollection<int> LevelScores;
    public LevelLeaderboardViewModel()
	{
	}
}