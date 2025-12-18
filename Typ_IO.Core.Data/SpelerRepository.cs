using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;

namespace Typ_IO.Core.Data
{
    public class SpelerRepository : ISpelerRepository
    {
        private readonly ISqliteConnectionFactory _factory;
        public SpelerRepository(ISqliteConnectionFactory factory) => _factory = factory;

        public async Task AddAsync(Speler speler, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Speler (Naam) VALUES ($naam); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$naam", speler.Naam);
            var writer = await cmd.ExecuteScalarAsync(ct);
            if (writer is long)
                speler.GetType().GetProperty("Id")?.SetValue(speler, Convert.ToInt32(writer));
        }

        public async Task<Speler?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Naam FROM Speler WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = await cmd.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                var speler = new Speler
                {
                    Naam = reader.GetString(1)
                };
                speler.GetType().GetProperty("Id")?.SetValue(speler, reader.GetInt32(0));
                return speler;
            }
            return null;
        }

        public async Task<List<Speler>> ListAsync(CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Naam FROM Speler ORDER BY Id;";
            using var reader = await cmd.ExecuteReaderAsync(ct);

            var list = new List<Speler>();
            while (await reader.ReadAsync(ct))
            {
                var speler = new Speler
                {
                    Naam = reader.GetString(1)
                };
                speler.GetType().GetProperty("Id")?.SetValue(speler, reader.GetInt32(0));
                list.Add(speler);
            }
            return list;
        }

        public async Task UpdateAsync(Speler speler, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Speler SET Naam=$naam WHERE Id=$id;";
            cmd.Parameters.AddWithValue("$id", speler.GetType().GetProperty("Id")?.GetValue(speler));
            cmd.Parameters.AddWithValue("$naam", speler.Naam);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Speler WHERE Id=$id;";
            cmd.Parameters.AddWithValue("$id", id);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
