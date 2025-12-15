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
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SL.Naam, SL.Topscore FROM S Standaardlevel JOIN SL SpelerLevel on S.id = SL.levelid ORDER BY SP.Topscore;";
            using var reader = await cmd.ExecuteReaderAsync(ct);

            var list = new List<Standaardlevel>();
            while (await reader.ReadAsync(ct))
            {
                var level = new Standaardlevel
                {
                    Naam = reader.GetString(1),
                    Tekst = reader.GetString(2),
                    Moeilijkheidsgraad = reader.GetInt32(3)
                };
                level.GetType().GetProperty("Id")?.SetValue(level, reader.GetInt32(0));
                list.Add(level);
            }
            return list;
        }

        public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
