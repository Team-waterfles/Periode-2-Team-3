using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;

namespace Typ_IO.Core.Data
{
    public class LeaderboardRepository: ILeaderboardRepository
    {
        private readonly ISqliteConnectionFactory _factory;
        public LeaderboardRepository(ISqliteConnectionFactory factory) => _factory = factory;

        public async Task<List<LevelScore>> GetLevelleaderboardAsync(int level_id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
