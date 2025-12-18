using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;

namespace Typ_IO.Core.Services
{
    public class LeaderboardService
    {
        private readonly ILevelleaderboardRepository _leaderboard;
        LeaderboardService()
        {
            _leaderboard = Application.Current.Windows[0].Page.Handler.MauiContext.Services.GetService<ILevelleaderboardRepository>();
        }
        public void haal_level(int level_id, int speler_id, int score)
        {
            int topscore = _leaderboard.GetScoreAsync(level_id, speler_id).Result;
            if (topscore >= score) { return; }

            if (topscore == 0)
            { _leaderboard.AddAsync(new SpelerLevel { LevelId = level_id, SpelerId = speler_id, TopScore = score }); }
            else { _leaderboard.UpdateAsync(new SpelerLevel { LevelId = level_id, SpelerId = speler_id, TopScore = score }); }
        }
    }
}
