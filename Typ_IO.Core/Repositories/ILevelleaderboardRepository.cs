using Typ_IO.Core.Models;

namespace Typ_IO.Core.Repositories
{
    public interface ILevelleaderboardRepository
    {
        Task AddAsync(SpelerLevel speler_level, CancellationToken ct = default);
        Task UpdateAsync(SpelerLevel speler_level, CancellationToken ct = default);
        Task DeleteAsync(SpelerLevel speler_level, CancellationToken ct = default);
        Task<List<SpelerLevel>> GetLeaderboardAsync(int level_id, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
