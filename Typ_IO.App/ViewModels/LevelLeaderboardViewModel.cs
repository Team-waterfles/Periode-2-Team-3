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
    public ObservableCollection<LevelScore> Levelscores = [];
    private readonly ISpelerRepository _spelerrepository;
    private readonly ILevelleaderboardRepository _leaderboardrepository;

    public LevelLeaderboardViewModel(Standaardlevel level)
    {
        _leaderboardrepository = Application.Current.Windows[0].Page.Handler.MauiContext.Services.GetService<ILevelleaderboardRepository>();
        _spelerrepository = Application.Current.Windows[0].Page.Handler.MauiContext.Services.GetService<ISpelerRepository>();
        GetLeaderboard(level);
    }
    public void GetLeaderboard(Standaardlevel level)
    {
        List<LevelScore> leaderboarddata = _leaderboardrepository.GetLeaderboardAsync(level.Id).Result;
        int positie = 1;
        foreach (LevelScore scoredata in leaderboarddata)
        {
            Levelscores.Add(new LevelScore(scoredata.Naam, positie, scoredata.TopScore));
            positie++;
        }
	}
}