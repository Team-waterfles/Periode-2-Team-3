using Typ_IO.Core.Models;

namespace Typ_IO.Core.Repositories
{
    public interface ILeaderboardRepository
    {
        Task<List<LevelScore>?> GetLevelleaderboardAsync(int level_id, CancellationToken ct = default);
    }
}
