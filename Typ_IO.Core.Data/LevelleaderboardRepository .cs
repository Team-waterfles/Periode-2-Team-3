using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;
using Windows.Devices.SmartCards;

namespace Typ_IO.Core.Data
{
    public class LevelleaderboardRepository: ILevelleaderboardRepository
    {
        private readonly ISqliteConnectionFactory _factory;
        public LevelleaderboardRepository(ISqliteConnectionFactory factory) => _factory = factory;

        public async Task AddAsync(SpelerLevel speler_level, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO SpelerLevel (LevelId, SpelerId, Topscore) VALUES ($level_id, $speler_id, $topscore); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$level_id", speler_level.LevelId);
            cmd.Parameters.AddWithValue("$speler_id", speler_level.SpelerId);
            cmd.Parameters.AddWithValue("$topscore", speler_level.TopScore);
            var writer = await cmd.ExecuteScalarAsync(ct);
        }
        public async Task UpdateAsync(SpelerLevel speler_level, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE SpelerLevel SET Topscore=$topscore WHERE LevelId=$level_id AND SpelerId=$speler_id;";
            cmd.Parameters.AddWithValue("$level_id", speler_level.LevelId);
            cmd.Parameters.AddWithValue("$speler_id", speler_level.SpelerId);
            cmd.Parameters.AddWithValue("$topscore", speler_level.TopScore);
            await cmd.ExecuteNonQueryAsync(ct);
        }
        public async Task DeleteAsync(SpelerLevel speler_level, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SpelerLevel WHERE LevelId=$level_id AND SpelerId=$speler_id;";
            cmd.Parameters.AddWithValue("$level_id", speler_level.LevelId);
            cmd.Parameters.AddWithValue("$speler_id", speler_level.SpelerId);
            await cmd.ExecuteNonQueryAsync(ct);
        }
        public async Task<List<SpelerLevel>> GetLeaderboardAsync(int level_id, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT S.Naam, SL.Topscore FROM Speler S JOIN SpelerLevel SL on S.Id = SL.SpelerId WHERE SL.LevelId = $level_id ORDER BY SL.Topscore;";
            cmd.Parameters.AddWithValue("$level_id", level_id);
            using var reader = await cmd.ExecuteReaderAsync(ct);

            var list = new List<SpelerLevel>();
            while (await reader.ReadAsync(ct))
            {
                var speler_level = new SpelerLevel
                {
                    TopScore = reader.GetInt32(2)
                };
                speler_level.GetType().GetProperty("LevelId")?.SetValue(speler_level, reader.GetInt32(0));
                speler_level.GetType().GetProperty("SpelerId")?.SetValue(speler_level, reader.GetInt32(1));
                list.Add(speler_level);
            }
            return list;
        }

        public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
