using Typ_IO.Core.Models;

namespace Typ_IO.Core.Repositories
{
    public interface ILevelleaderboardRepository
    {
        Task AddAsync(LevelScore score, CancellationToken ct = default);
        Task UpdateAsync(LevelScore score, CancellationToken ct = default);
        Task DeleteAsync(LevelScore score, CancellationToken ct = default);
        Task<List<LevelScore>?> GetLeaderboardAsync(int level_id, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
